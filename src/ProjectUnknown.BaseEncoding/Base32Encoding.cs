namespace ProjectUnknown.BaseEncoding
{
    public class Base32Encoding : BaseEncoding
    {
        public Base32Encoding(string characterSet, char? paddingCharacter, bool mostSignificantByteFirst = true) : base(5, characterSet, paddingCharacter, mostSignificantByteFirst)
        {
        }

        public static Base32Encoding Default { get; } = new Base32Encoding("ABCDEFGHIJKLMNOPQRSTUVWXYZ234567", '=');
        
        public static Base32Encoding Crockford { get; } = new Base32Encoding("0123456789ABCDEFGHJKMNPQRSTVWXYZ", '=');
    }
}
