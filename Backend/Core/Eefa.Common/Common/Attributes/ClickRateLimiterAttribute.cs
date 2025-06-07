using Eefa.Common.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Eefa.Common.Common.Attributes
{
    public class ClickRateLimiterAttribute : ActionFilterAttribute
    {
        private readonly int _AllowedClickIntervalSeconds = 5000;
        private readonly int _AllowedClickCount = 1;
  
        public ClickRateLimiterAttribute(int allowedClickIntervalSeconds , int allowedClickCount)
        {
            _AllowedClickIntervalSeconds = allowedClickIntervalSeconds;
            _AllowedClickCount = allowedClickCount;
        }
 

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var clickRateLimiter = context.HttpContext.RequestServices.GetRequiredService<ClickRateLimiter>();
            int userId = GetUserIdFromContext(context); // متدی برای گرفتن شناسه‌ی منحصربه‌فرد کاربر از context

            if (!clickRateLimiter.CanSubmit(userId, _AllowedClickIntervalSeconds, _AllowedClickCount))
            {
              context.Result =new OkObjectResult(ServiceResult.Failed(null));
            }
        }

        private int GetUserIdFromContext(ActionExecutingContext context)
        {
            return int.Parse(context.HttpContext.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "id")?.Value ?? "2");
        }
    }
}
