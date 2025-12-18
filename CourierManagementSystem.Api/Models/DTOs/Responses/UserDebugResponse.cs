using CourierManagementSystem.Api.Models.Entities;

namespace CourierManagementSystem.Api.Models.DTOs.Responses
{
    public class UserDebugResponse
    {
        public long? Id { get; set; }
        public string? Login { get; set; }
        public UserRole? Role { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
