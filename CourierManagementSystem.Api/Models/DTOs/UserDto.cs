using CourierManagementSystem.Api.Models.Entities;

namespace CourierManagementSystem.Api.Models.DTOs;

public class UserDto
{
    public long Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }

    public static UserDto From(User user) 
        => user.ToDto();
}
