namespace ProjectUnknown.BaseEncoding
{
    public class Base64Encoding : BaseEncoding
    {
        public Base64Encoding(string characterSet, char? paddingCharacter, bool mostSignificantByteFirst = true) : base(
            6, characterSet, paddingCharacter, mostSignificantByteFirst)
        {
        }

        public static Base64Encoding Default { get; } =
            new Base64Encoding("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/", '=');

        public static Base64Encoding Blowfish { get; } =
            new Base64Encoding("./ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", null);

        // ReSharper disable once InconsistentNaming
        public static Base64Encoding MD5 { get; } =
            new Base64Encoding("./0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", null, false);
    }
}
