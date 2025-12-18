using CourierManagementSystem.Api.Models.Entities;

namespace CourierManagementSystem.Api.Models.DTOs.Responses
{
    public class UserDebugResponse
    {
        public string? Id { get; set; }
        public string? Login { get; set; }
        public string? Role { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
