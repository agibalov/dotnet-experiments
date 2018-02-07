using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AspNetMvcWebApiExperiment.CustomValidationTests
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var modelState = actionContext.ModelState;
            if (modelState.IsValid)
            {
                return;
            }

            var errors = new Dictionary<string, string>();
            foreach (var key in modelState.Keys)
            {
                var state = modelState[key];
                if (!state.Errors.Any())
                {
                    continue;
                }

                errors[key] = state.Errors.First().ErrorMessage;
            }

            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errors);
        }
    }
}