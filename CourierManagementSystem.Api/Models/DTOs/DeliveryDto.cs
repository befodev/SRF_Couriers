using CourierManagementSystem.Api.Models.Entities;

namespace CourierManagementSystem.Api.Models.DTOs;

public class DeliveryDto
{
    public long Id { get; set; }
    public string? DeliveryNumber { get; set; }
    public UserDto? Courier { get; set; }
    public VehicleDto? Vehicle { get; set; }
    public UserDto CreatedBy { get; set; } = null!;
    public DateOnly DeliveryDate { get; set; }
    public TimeOnly TimeStart { get; set; }
    public TimeOnly TimeEnd { get; set; }
    public DeliveryStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<DeliveryPointDto> DeliveryPoints { get; set; } = new();
    public decimal TotalWeight { get; set; }
    public decimal TotalVolume { get; set; }
    public bool CanEdit { get; set; }

    public static DeliveryDto From(Delivery delivery) 
        => delivery.ToDto();
}
