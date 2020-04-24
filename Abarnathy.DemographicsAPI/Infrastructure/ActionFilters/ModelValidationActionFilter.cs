using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Abarnathy.DemographicsAPI.Infrastructure.ActionFilters
{
    public class ModelValidationActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}