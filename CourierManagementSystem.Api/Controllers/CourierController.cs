using CourierManagementSystem.Api.Attributes;
using CourierManagementSystem.Api.Models.Entities;
using CourierManagementSystem.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourierManagementSystem.Api.Controllers;

[ApiController]
[Route("courier")]
[Authorize(Roles = "courier")]
public class CourierController : ControllerBase
{
    private readonly ICourierService _courierService;

    public CourierController(ICourierService courierService)
    {
        _courierService = courierService;
    }

    [HttpGet("deliveries")]
    [ExtractUserId]
    public async Task<IActionResult> GetDeliveries(
        [FromQuery] DateOnly? date,
        [FromQuery] DeliveryStatus? status,
        [FromQuery] DateOnly? dateFrom,
        [FromQuery] DateOnly? dateTo)
    {
        var courierId = (long) HttpContext.Items["UserId"]!;
        var deliveries = await _courierService.GetCourierDeliveriesAsync(courierId, date, status, dateFrom, dateTo);
        return Ok(deliveries);
    }

    [HttpGet("deliveries/{id}")]
    [ExtractUserId]
    public async Task<IActionResult> GetDeliveryById(long id)
    {
        var courierId = (long) HttpContext.Items["UserId"]!;
        var delivery = await _courierService.GetCourierDeliveryByIdAsync(courierId, id);
        return Ok(delivery);
    }
}
