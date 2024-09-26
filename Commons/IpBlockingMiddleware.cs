using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dragon.Bot
{
    public class IpBlockingMiddleware : ActionFilterAttribute
    {
        public IpBlockingMiddleware()
        {
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;

            bool isUnBlocked;

            if (remoteIpAddress is not null)
            {
                return;
                isUnBlocked = new List<string> { "52.89.214.238", "34.212.75.30", "54.218.53.128", "52.32.178.7" }.Contains(remoteIpAddress.Address.ToString());
            }
            else
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }

            if (!isUnBlocked)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
