using CourierManagementSystem.Api.Models.DTOs.Responses;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace CourierManagementSystem.Api.Services
{
    public class UserContextService : IUserContextService
    {
        public UserDebugResponse GetUserDebugInfo(ClaimsPrincipal user)
        {
            if (user?.Identity?.IsAuthenticated != true)
                return new UserDebugResponse { IsAuthenticated = false };

            return new UserDebugResponse
            {
                Id = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
                Login = user.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value,
                Role = user.FindFirst(ClaimTypes.Role)?.Value,
                IsAuthenticated = true
            };
        }
    }
}
