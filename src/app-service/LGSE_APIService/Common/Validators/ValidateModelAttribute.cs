
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace LGSE_APIService.Validators
{
    /// <summary>
    /// [ValidateModel] will be decorated to Controller public methods for which
    /// ModelState validation is required
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null) throw new ArgumentNullException();
            //if (actionContext == null) throw new ArgumentNullException(nameof(actionContext));
            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        } 
    }
}
