using System.Reflection;
using System.Security.Claims;
using Avvo.Core.Commons.Entities;
using Avvo.Core.Host.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Host.Base
{
    public class ApiBaseController : Controller
    {
        public UserIdentity UserIdentity { get; set; }
        public string AccessToken { get; set; }
        public string RefreshAccessToken { get; set; }
        private readonly ILogger _logger;
        private readonly HttpClient _client = new HttpClient();
        private const string dataingest_eventname = "backend_event";
        private const string ResponseTimeKey = "ResponseTimeKey";
        private const string RequestIdKey = "RequestIdKey";
        private const string StartDateKey = "StartDateKey";

        public ApiBaseController(ILogger logger)
        {
            _logger = logger;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private void LoadAccessToken(ActionExecutingContext context)
        {
            try
            {
                var req = context.HttpContext.Request;
                var accessToken = req.Headers["Authorization"];
                this.AccessToken = accessToken.ToString().Replace("Bearer ", "");

                if (req.Headers.ContainsKey("x-refresh-token"))
                {
                    this.RefreshAccessToken = req.Headers["x-refresh-token"];
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"{this.GetType().Name}_{MethodBase.GetCurrentMethod().Name} : Could not load acess token");
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private void LoadCustomIdentity()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            try
            {
                UserIdentity = identity.GetIdentity<UserIdentity>(_logger);
                UserIdentity.RefreshAccessToken = this.RefreshAccessToken;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"{this.GetType().Name}_{MethodBase.GetCurrentMethod().Name} : Could not load CustomIdentity");
            }
        }
    }
}
