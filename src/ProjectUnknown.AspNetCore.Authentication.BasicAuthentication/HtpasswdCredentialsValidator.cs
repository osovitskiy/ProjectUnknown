using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Isopoh.Cryptography.Argon2;

using ProjectUnknown.BaseEncoding;

namespace ProjectUnknown.AspNetCore.Authentication.BasicAuthentication
{
    public class HtpasswdCredentialsValidator : ICredentialsValidator
    {
        private interface IPasswordHash
        {
            bool Validate(string password);
        }

        private enum HashParseResult
        {
            Success,
            InvalidPrefix,
            InvalidFormat,
        }

        private delegate HashParseResult HashParser<THash>(string text, out THash hash) where THash : IPasswordHash;

        private class Argon2PasswordHash : IPasswordHash
        {
            private readonly string _encodedHash;

            public Argon2PasswordHash(string encodedHash)
            {
                _encodedHash = encodedHash;
            }

            public bool Validate(string password)
            {
                return Argon2.Verify(_encodedHash, password);
            }

            private static readonly string[] Prefixes = new []{"$argon2d$", "$argon2i$", "$argon2id$"};

            public static HashParseResult TryParse(string encodedHash, out Argon2PasswordHash parsed)
            {
                if (!Prefixes.Any(prefix => encodedHash.StartsWith(prefix, StringComparison.Ordinal)))
                {
                    parsed = null;
                    return HashParseResult.InvalidPrefix;
                }

                var config = new Argon2Config();
                if (config.DecodeString(encodedHash, out var hash))
                {
                    parsed = new Argon2PasswordHash(encodedHash);
                    return HashParseResult.Success;
                }
                else
                {
                    parsed = null;
                    return HashParseResult.InvalidFormat;
                }
            }
        }

        private class Apr1PasswordHash : IPasswordHash
        {
            private readonly byte[] _salt;
            private readonly byte[] _hash;

            public Apr1PasswordHash(byte[] salt, byte[] hash)
            {
                _salt = salt;
                _hash = hash;
            }

            public bool Validate(string password)
            {
                return Apr1Hash.Validate(password, _salt, _hash);
            }

            private const string Prefix = "$apr1$";
            private const int SaltLength = 8;
            private const int PasswordLength = 22;

            public static HashParseResult TryParse(string encodedHash, out Apr1PasswordHash parsed)
            {
                parsed = null;
                if (!encodedHash.StartsWith(Prefix, StringComparison.Ordinal))
                {
                    return HashParseResult.InvalidPrefix;
                }

                if (encodedHash.Length != Prefix.Length + SaltLength + 1 + PasswordLength ||
                    encodedHash[Prefix.Length + SaltLength] != '$')
                {
                    return HashParseResult.InvalidFormat;
                }

                var salt = encodedHash.Substring(Prefix.Length, SaltLength);
                if (!Base64Encoding.MD5.IsValid(salt))
                {
                    return HashParseResult.InvalidFormat;
                }

                var password = encodedHash.Substring(Prefix.Length + SaltLength + 1);
                if (!Base64Encoding.MD5.TryDecode(password, out var hash))
                {
                    return HashParseResult.InvalidFormat;
                }

                parsed = new Apr1PasswordHash(Encoding.ASCII.GetBytes(salt), hash);
                return HashParseResult.Success;
            }
        }

        private class BCryptPasswordHash : IPasswordHash
        {
            private readonly string _hash;

            public BCryptPasswordHash(string hash)
            {
                _hash = hash;
            }

            public bool Validate(string password)
            {
                return BCrypt.Net.BCrypt.Verify(password, _hash);
            }

            private const string Prefix = "2y";
            private const int SaltLength = 2;
            private const int PasswordLength = 54;

            public static HashParseResult TryParse(string encodedHash, out BCryptPasswordHash parsed)
            {
                parsed = null;
                if (!encodedHash.StartsWith(Prefix, StringComparison.Ordinal))
                {
                    return HashParseResult.InvalidPrefix;
                }

                if (encodedHash.Length != Prefix.Length + SaltLength + 1 + PasswordLength ||
                    encodedHash[Prefix.Length + SaltLength] != '$')
                {
                    return HashParseResult.InvalidFormat;
                }

                var salt = encodedHash.Substring(Prefix.Length, SaltLength);
                if (salt.Any(x => !char.IsDigit(x)))
                {
                    return HashParseResult.InvalidFormat;
                }

                var password = encodedHash.Substring(Prefix.Length + SaltLength + 1);
                if (!Base64Encoding.Blowfish.IsValid(password))
                {
                    return HashParseResult.InvalidFormat;
                }

                parsed = new BCryptPasswordHash(encodedHash);
                return HashParseResult.Success;
            }
        }

        private readonly Dictionary<string, IPasswordHash> _passwords =
            new Dictionary<string, IPasswordHash>();

        public HtpasswdCredentialsValidator(string path) : this(path, Array.Empty<string>()){}

        public HtpasswdCredentialsValidator(string path, string[] commentPrefixes)
        {
            using (var stream = File.OpenText(path))
            {
                var i = 0;
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    i++;
                    if (string.IsNullOrWhiteSpace(line) ||
                        commentPrefixes.Any(prefix => line.StartsWith(prefix, StringComparison.Ordinal)))
                    {
                        continue;
                    }

                    var delimiter = line.IndexOf(':');
                    if (delimiter < 0)
                    {
                        throw new Exception($"Missing username password delimiter on line {i}");
                    }

                    var username = line.Substring(0, delimiter);
                    if (_passwords.ContainsKey(username))
                    {
                        throw new Exception($"Duplicate username '{username}' on line {i}");
                    }

                    var hashedPassword = line.Substring(delimiter + 1);
                    bool TryParse<T>(HashParser<T> parser) where T : IPasswordHash
                    {
                        switch (parser(hashedPassword, out var hash))
                        {
                            case HashParseResult.Success:
                                _passwords.Add(username, hash);
                                return true;
                            case HashParseResult.InvalidPrefix:
                                return false;
                            case HashParseResult.InvalidFormat:
                                throw new Exception($"Invalid password hash format on line {i}");
                            default:
                                throw new NotSupportedException();
                        }
                    }

                    if (TryParse<Argon2PasswordHash>(Argon2PasswordHash.TryParse) ||
                        TryParse<Apr1PasswordHash>(Apr1PasswordHash.TryParse) ||
                        TryParse<BCryptPasswordHash>(BCryptPasswordHash.TryParse))
                    {
                        continue;
                    }

                    throw new Exception($"Unrecognized password hash format on line {i}");
                }
            }
        }

        public Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            if (!_passwords.TryGetValue(username, out var hash))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(hash.Validate(password));
        }
    }
}
