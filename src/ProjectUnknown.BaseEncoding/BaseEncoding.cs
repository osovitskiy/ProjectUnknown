using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectUnknown.BaseEncoding
{
    public abstract class BaseEncoding
    {
        private readonly int _bitsPerCharacter;
        private readonly string _characterSet;
        private readonly char? _paddingCharacter;
        private readonly bool _mostSignificantByteFirst;

        private readonly int _charAlignedBlockSize;
        private readonly int _byteAlignedBlockSize;
        private readonly int _byteMask;

        private readonly Dictionary<char, int> _characterMap;

        protected BaseEncoding(int bitsPerCharacter, string characterSet, char? paddingCharacter,
            bool mostSignificantByteFirst = true)
        {
            if (characterSet == null)
            {
                throw new NullReferenceException(nameof(characterSet));
            }

            switch (bitsPerCharacter)
            {
                case 4:
                    _charAlignedBlockSize = 4;
                    _byteAlignedBlockSize = 2;
                    _byteMask = 0x0F;
                    break;
                case 5:
                    _charAlignedBlockSize = 8;
                    _byteAlignedBlockSize = 5;
                    _byteMask = 0x1F;
                    break;
                case 6:
                    _charAlignedBlockSize = 4;
                    _byteAlignedBlockSize = 3;
                    _byteMask = 0x3F;
                    break;
                default:
                    throw new ArgumentException("Unsupported bits per character value");
            }

            if (characterSet.Length != 1 << bitsPerCharacter)
            {
                throw new ArgumentException($"Character set size must be {1 << bitsPerCharacter}",
                    nameof(characterSet));
            }

            _characterMap = new Dictionary<char, int>(characterSet.Length);

            var charIdx = 0;
            foreach (var character in characterSet)
            {
                if (_characterMap.ContainsKey(character))
                {
                    throw new ArgumentException("Duplicate characters in character set",
                        nameof(characterSet));
                }

                _characterMap[character] = charIdx++;
            }

            _bitsPerCharacter = bitsPerCharacter;
            _characterSet = characterSet;
            _paddingCharacter = paddingCharacter;
            _mostSignificantByteFirst = mostSignificantByteFirst;
        }

        public string Encode(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length == 0)
            {
                return string.Empty;
            }

            int length = _paddingCharacter.HasValue
                ? ((bytes.Length - 1) / _byteAlignedBlockSize + 1) * _charAlignedBlockSize
                : (bytes.Length * 8 - 1) / _bitsPerCharacter + 1;
            var result = new StringBuilder(length);

            int state = 0, offset = 0;
            foreach (var @byte in bytes)
            {
                if (_mostSignificantByteFirst)
                {
                    state = (state << 8) | @byte;
                    offset += 8;
                    while (offset >= _bitsPerCharacter)
                    {
                        offset -= _bitsPerCharacter;
                        result.Append(_characterSet[(state >> offset) & _byteMask]);
                    }
                }
                else
                {
                    state |= @byte << offset;
                    offset += 8;
                    while (offset >= _bitsPerCharacter)
                    {
                        result.Append(_characterSet[state & _byteMask]);
                        offset -= _bitsPerCharacter;
                        state >>= _bitsPerCharacter;
                    }
                }
            }

            if (offset > 0)
            {
                if (_mostSignificantByteFirst)
                {
                    result.Append(_characterSet[(state << (_bitsPerCharacter - offset)) & _byteMask]);
                }
                else
                {
                    result.Append(_characterSet[state & _byteMask]);
                }
            }

            if (_paddingCharacter.HasValue)
            {
                result.Append(_paddingCharacter.Value, length - result.Length);
            }

            return result.ToString();
        }

        public byte[] Decode(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text == string.Empty)
            {
                return Array.Empty<byte>();
            }

            if (!TryValidateNonEmpty(text, out var lastNonPaddingCharacterIdx, out var error))
            {
                throw new ArgumentException(error, nameof(text));
            }

            return DecodeValidNonEmpty(text, lastNonPaddingCharacterIdx);
        }

        public bool TryDecode(string text, out byte[] bytes)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text == string.Empty)
            {
                bytes = Array.Empty<byte>();
                return true;
            }

            if (!TryValidateNonEmpty(text, out var lastNonPaddingCharacterIdx, out _))
            {
                bytes = null;
                return false;
            }

            bytes = DecodeValidNonEmpty(text, lastNonPaddingCharacterIdx);
            return true;
        }

        public bool IsValid(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text == string.Empty)
            {
                return true;
            }

            return TryValidateNonEmpty(text, out _, out _);
        }

        private bool TryValidateNonEmpty(string text, out int lastNonPaddingCharacterIdx, out string error)
        {
            lastNonPaddingCharacterIdx = -1;
            error = null;

            if (_paddingCharacter.HasValue && text.Length % _charAlignedBlockSize != 0)
            {
                error =
                    $"Input string length must be multiple of {_bitsPerCharacter}";
                return false;
            }

            lastNonPaddingCharacterIdx = text.Length - 1;
            if (_paddingCharacter.HasValue)
            {
                var startOfLastBlock = (text.Length - 1) - (text.Length - 1) % _charAlignedBlockSize;
                for (; lastNonPaddingCharacterIdx >= startOfLastBlock; lastNonPaddingCharacterIdx--)
                {
                    if (text[lastNonPaddingCharacterIdx] != _paddingCharacter.Value)
                    {
                        break;
                    }
                }

                if (lastNonPaddingCharacterIdx < startOfLastBlock)
                {
                    error =
                        $"Input string cannot have more than {_bitsPerCharacter - 1} padding characters";
                    return false;
                }
            }

            for (var i = 0; i <= lastNonPaddingCharacterIdx; i++)
            {
                if (!_characterMap.ContainsKey(text[i]))
                {
                    error = "Unexpected character";
                    return false;
                }
            }

            return true;
        }

        private byte[] DecodeValidNonEmpty(string text, int lastNonPaddingCharacterIdx)
        {
            var result = new byte[lastNonPaddingCharacterIdx * _bitsPerCharacter / 8 + 1];

            int index = 0, state = 0, offset = 0;
            for (var i = 0; i <= lastNonPaddingCharacterIdx; i++)
            {
                var @byte = _characterMap[text[i]];
                if (_mostSignificantByteFirst)
                {
                    state = state << _bitsPerCharacter | @byte;
                    offset += _bitsPerCharacter;
                    if (offset >= 8)
                    {
                        offset -= 8;
                        result[index++] = (byte) ((state >> offset) & 0xFF);
                    }
                }
                else
                {
                    state |= (@byte << offset);
                    offset += _bitsPerCharacter;
                    if (offset >= 8)
                    {
                        result[index++] = (byte) (state & 0xFF);
                        offset -= 8;
                        state >>= 8;
                    }
                }
            }

            return result;
        }
    }
}
