using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectUnknown.AspNetCore.OAuth.Swagger
{
    public class OAuthDocumentFilter : IDocumentFilter
    {
        public OAuthDocumentFilter(PathString tokenEndpoint)
        {
            TokenEndpoint = tokenEndpoint;
        }

        public PathString TokenEndpoint { get; set; }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths = GetOAuthPaths().Concat(swaggerDoc.Paths).ToDictionary(x => x.Key, x => x.Value);
        }

        private IDictionary<string, PathItem> GetOAuthPaths() => new Dictionary<string, PathItem>()
        {
            {
                TokenEndpoint, new PathItem()
                {
                    Post = new Operation()
                    {
                        Tags = new List<string>() {"OAuth"},
                        Consumes = new List<string>() {"application/x-www-urlformencoded"},
                        Produces = new List<string>() {"application/json"},
                        Parameters = new List<IParameter>()
                        {
                            new NonBodyParameter()
                            {
                                Name = "grant_type",
                                In = "formData",
                                Type = "string",
                                Enum = new List<object>()
                                {
                                    "password",
                                    "refresh_token"
                                },
                                Required = true
                            },
                            new NonBodyParameter()
                            {
                                Name = "username",
                                In = "formData",
                                Type = "string"
                            },
                            new NonBodyParameter()
                            {
                                Name = "password",
                                In = "formData",
                                Type = "string"
                            },
                            new NonBodyParameter()
                            {
                                Name = "refresh_token",
                                In = "formData",
                                Type = "string"
                            }
                        },
                        Responses = new Dictionary<string, Response>()
                        {
                            {
                                "200", new Response()
                                {
                                    Description = "Success",
                                    Schema = new Schema()
                                    {
                                        Type = "object",
                                        Properties = new Dictionary<string, Schema>()
                                        {
                                            {
                                                "access_token", new Schema()
                                                {
                                                    Type = "string"
                                                }
                                            },
                                            {
                                                "token_type", new Schema()
                                                {
                                                    Type = "string"
                                                }
                                            },
                                            {
                                                "expires_in", new Schema()
                                                {
                                                    Type = "integer"
                                                }
                                            },
                                            {
                                                "expires_at", new Schema()
                                                {
                                                    Type = "string"
                                                }
                                            },
                                            {
                                                "refresh_token", new Schema()
                                                {
                                                    Type = "string"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
    }
}
