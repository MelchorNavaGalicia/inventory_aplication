using inventory_aplication.Application.Features.Common.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace inventory_aplication.Application.Features.Common.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorMessages = string.Join(" | ", context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(e => e.ErrorMessage));

                context.Result = new BadRequestObjectResult(
                    Result<string>.Fail(errorMessages, ErrorCodes.BadRequest)
                );
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
