using CourierManagementSystem.Api.Models.DTOs.Responses;
using System.Security.Claims;

namespace CourierManagementSystem.Api.Services
{
    public interface IUserContextService
    {
        UserDebugResponse GetUserDebugInfo(ClaimsPrincipal user);
    }
}
