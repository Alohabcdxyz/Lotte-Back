using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LotteriaAPI
{
  public class SwaggerReferenceRemoveFilter : IOperationFilter
  {
    void IOperationFilter.Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      operation.Parameters = null;
      operation.Responses.Clear();
    }
  }
}
