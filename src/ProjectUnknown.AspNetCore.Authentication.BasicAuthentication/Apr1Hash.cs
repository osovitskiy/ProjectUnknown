using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using ProjectUnknown.AspNetCore.Authentication.BasicAuthentication.Extensions;

namespace ProjectUnknown.AspNetCore.Authentication.BasicAuthentication
{
    public class Apr1Hash
    {
        private static readonly byte[] ZeroByte = new byte[1];
        private static readonly byte[] PrefixBytes = Encoding.ASCII.GetBytes("$apr1$");
        private static readonly int[] Permutations = {12, 6, 0, 13, 7, 1, 14, 8, 2, 15, 9, 3, 5, 10, 4, 11};

        public static bool Validate(string password, byte[] salt, byte[] hash)
        {
            byte[] key = null, crypt = null;
            try
            {
                key = Encoding.UTF8.GetBytes(password);
                crypt = Crypt(key, salt);
                return SecureCompare(hash, crypt);
            }
            finally
            {
                SecureClear(key);
                SecureClear(crypt);
            }
        }

        private static byte[] Crypt(byte[] key, byte[] salt)
        {
            var md5 = MD5.Create();
            var md5HashSize = md5.HashSize / 8;
            byte[] init = new byte[md5HashSize], hash = new byte[md5HashSize];

            try
            {
                md5.Initialize();
                md5.AddToDigest(key);
                md5.AddToDigest(salt);
                md5.AddToDigest(key);
                md5.FinishDigest();
                md5.Hash.CopyTo(init, 0);

                md5.Initialize();
                md5.AddToDigest(key);
                md5.AddToDigest(PrefixBytes);
                md5.AddToDigest(salt);

                for (int i = 0; i < key.Length; i += init.Length)
                {
                    md5.AddToDigest(init, 0, Math.Min(key.Length - i, init.Length));
                }

                int length = key.Length;
                for (int i = 0; i < 31 && length != 0; i++)
                {
                    if ((length & (1 << i)) != 0)
                    {
                        md5.AddToDigest(ZeroByte);
                    }
                    else
                    {
                        md5.AddToDigest(key, 0, 1);
                    }
                    length &= ~(1 << i);
                }

                md5.FinishDigest();
                md5.Hash.CopyTo(hash, 0);

                for (int i = 0; i < 1000; i++)
                {
                    md5.Initialize();
                    if ((i & 1) != 0)
                    {
                        md5.AddToDigest(key);
                    }

                    if ((i & 1) == 0)
                    {
                        md5.AddToDigest(hash);
                    }

                    if ((i % 3) != 0)
                    {
                        md5.AddToDigest(salt);
                    }

                    if ((i % 7) != 0)
                    {
                        md5.AddToDigest(key);
                    }

                    if ((i & 1) != 0)
                    {
                        md5.AddToDigest(hash);
                    }

                    if ((i & 1) == 0)
                    {
                        md5.AddToDigest(key);
                    }

                    md5.FinishDigest();
                    md5.Hash.CopyTo(hash, 0);
                }

                var crypt = new byte[md5HashSize];
                for (int i = 0; i < md5HashSize; i++)
                {
                    crypt[i] = hash[Permutations[i]];
                }
                return crypt;
            }
            finally
            {
                md5.Clear();
                SecureClear(init);
                SecureClear(hash);
            }
        }

        private static void SecureClear(Array array)
        {
            if (array != null)
            {
                Array.Clear(array, 0, array.Length);
            }
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool SecureCompare(byte[] lhs, byte[] rhs)
        {
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if (lhs == null || rhs == null || lhs.Length != rhs.Length)
            {
                return false;
            }

            var result = true;
            for (var i = 0; i < lhs.Length; i++)
            {
                result &= (lhs[i] == rhs[i]);
            }
            return result;
        }
    }
}
