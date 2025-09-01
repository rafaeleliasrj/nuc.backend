using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Avvo.Core.Host.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionActionFilter : ActionFilterAttribute, IActionFilter
    {
        private readonly string[] _keys;
        public PermissionActionFilter(string[] keys)
        {
            _keys = keys;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var identity = (ClaimsIdentity)context.HttpContext.User.Identity;
            var claims = identity.Claims;
            var scopesClaim = claims.Where(c => c.Type.Contains("Scopes")).Select(q => q.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(scopesClaim))
            {
                var scopes = scopesClaim.Split(",");

                var isAuthorized = scopes.Where(a => _keys.Where(b => b == a).Any()).Any();

                if (!isAuthorized)
                    SetResponseError(context);
                else
                    base.OnActionExecuting(context);
            }
            else
                SetResponseError(context);
        }

        private void SetResponseError(ActionExecutingContext context)
        {
            context.HttpContext.Response.StatusCode = 403;
            context.HttpContext.Response.Headers.Clear();
            var error = new
            {
                statusCode = (int)HttpStatusCode.Forbidden,
                errorCode = "FORBIDDEN",
                messages = new System.Collections.Generic.List<string> { "Access denied" }
            };
            context.Result = new JsonResult(error);
        }
    }
}
