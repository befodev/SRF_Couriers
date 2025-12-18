using CourierManagementSystem.Api.Exceptions;
using CourierManagementSystem.Api.Models.DTOs.Responses;
using CourierManagementSystem.Api.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CourierManagementSystem.Api.Services
{
    public class UserContextService : IUserContextService
    {
        public UserDebugResponse GetUserDebugInfo(ClaimsPrincipal user)
        {
            if (user?.Identity?.IsAuthenticated != true)
                return new UserDebugResponse { IsAuthenticated = false };

            string? id = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
                    login = user.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value,
                    role = user.FindFirst(ClaimTypes.Role)?.Value;

            if (id is null || login is null || role is null)
                throw new NotFoundException("id/login/role is/are null");

            return new UserDebugResponse
            {
                Id = long.Parse(id),
                Login = login,
                Role = Enum.Parse<UserRole>(role),
                IsAuthenticated = true
            };
        }
    }
}
