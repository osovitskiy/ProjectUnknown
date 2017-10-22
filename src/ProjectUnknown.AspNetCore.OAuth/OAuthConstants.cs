namespace ProjectUnknown.AspNetCore.OAuth
{
    internal class OAuthConstants
    {
        public static class ParamNames
        {
            public const string GrantType = "grant_type";
            public const string Username = "username";
            public const string Password = "password";
            public const string RefreshToken = "refresh_token";
        }

        public static class GrantTypes
        {
            public const string Password = "password";
            public const string RefreshToken = "refresh_token";
        }
    }
}
