namespace ProjectUnknown.AspNetCore.OAuth
{
    public class OAuthResult
    {
        public int StatusCode { get; set; }
        public object Body { get; set; }

        public static OAuthResult InvalidRequest(string description)
        {
            return Error("invalid_request", description);
        }

        public static OAuthResult InvalidGrant(string description)
        {
            return Error("invalid_grant", description);
        }

        public static OAuthResult UnsupportedGrantType(string description)
        {
            return Error("unsupported_grant_type", description);
        }

        public static OAuthResult ServerError(string description)
        {
            return Error("server_error", description);
        }

        public static OAuthResult Error(string code, string description)
        {
            return new OAuthResult()
            {
                StatusCode = 400,
                Body = new OAuthError()
                {
                    Error = code,
                    ErrorDescription = description
                }
            };
        }

        public static OAuthResult Success()
        {
            return new OAuthResult()
            {
                StatusCode = 200
            };
        }

        public static OAuthResult Success(object value)
        {
            return new OAuthResult()
            {
                StatusCode = 200,
                Body = value
            };
        }
    }
}
