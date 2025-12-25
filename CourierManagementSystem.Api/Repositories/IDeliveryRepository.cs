using CourierManagementSystem.Api.Models.Entities;

namespace CourierManagementSystem.Api.Repositories;

public interface IDeliveryRepository
{
    Task<List<Delivery>> GetAllAsync();
    Task<List<Delivery>> GetByFilterAsync(DeliveryFilterSpecification filter);
    Task<Delivery?> GetByIdAsync(long id);
    Task<Delivery?> GetByIdWithDetailsAsync(long id);
    Task<List<Delivery>> GetByCourierWithDetailsAsync(long courierId);
    Task<List<Delivery>> GetByDeliveryDateAndCourierIdWithDetailsAsync(DateOnly date, long courierId);
    Task<List<Delivery>> GetByCourierIdAndDeliveryDateBetweenWithDetailsAsync(long courierId, DateOnly startDate, DateOnly endDate);
    Task<List<Delivery>> GetByDateVehicleAndOverlappingTimeAsync(DateOnly date, long vehicleId, TimeOnly startTime, TimeOnly endTime, long? excludeDeliveryId = null);
    Task<Delivery> CreateAsync(Delivery delivery);
    Task<Delivery> UpdateAsync(Delivery delivery);
    Task DeleteAsync(Delivery delivery);
    Task<int> SaveChangesAsync();
}

public class DeliveryFilterSpecification
{
    public DateOnly? Date { get; set; }
    public long? CourierId { get; set; }
    public DeliveryStatus? Status { get; set; }
    public bool IncludeDetails { get; set; } = true;
}