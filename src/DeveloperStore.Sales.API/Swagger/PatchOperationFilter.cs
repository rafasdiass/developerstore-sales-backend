using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace DeveloperStore.Sales.API.Swagger
{
    public class PatchOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasPatch = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<Microsoft.AspNetCore.Mvc.HttpPatchAttribute>()
                .Any();

            if (hasPatch)
            {
                operation.Summary ??= "Atualização parcial via PATCH";
                operation.OperationId ??= $"Patch_{context.MethodInfo.Name}";
            }
        }
    }
}
