using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace CourierManagementSystem.Api.Attributes
{
    public class ExtractUserIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            var userIdClaim = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim != null && long.TryParse(userIdClaim, out var userId))
            {
                context.HttpContext.Items["UserId"] = userId;
            }
            else if (context.Controller is ControllerBase controller)
            {
                context.Result = controller.Unauthorized();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}