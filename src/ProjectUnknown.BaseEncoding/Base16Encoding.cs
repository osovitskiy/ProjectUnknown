namespace ProjectUnknown.BaseEncoding
{
    public class Base16Encoding : BaseEncoding
    {
        public Base16Encoding(string characterSet, char? paddingCharacter, bool mostSignificantByteFirst = true) : base(4, characterSet, paddingCharacter, mostSignificantByteFirst)
        {
        }

        public static Base16Encoding Default { get; } = new Base16Encoding("0123456789ABCDEF", null);
    }
}
