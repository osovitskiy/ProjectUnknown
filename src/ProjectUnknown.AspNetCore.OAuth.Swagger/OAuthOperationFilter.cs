using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectUnknown.AspNetCore.OAuth.Swagger
{
    public class OAuthOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            operation.Parameters.Insert(0, new NonBodyParameter()
            {
                Name = "Authorization",
                In = "header",
                Type = "string",
                Description = "Access Token",
                Required = false
            });
        }
    }
}
