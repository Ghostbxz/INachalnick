using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace INachalnickTinyRestApi.Middlewares
{
    public class AuthenticationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isDebug = false;
            var key = "123";
            if (context.HttpContext.Request.Host.Host == "localhost" || isDebug)
            {
                return;
            }
            if (!context.HttpContext.Request.Query.ContainsKey("key") || context.HttpContext.Request.Query["key"] != key)
            {
                throw new UnauthorizedAccessException("bad key value");
            }
        }
    }
}