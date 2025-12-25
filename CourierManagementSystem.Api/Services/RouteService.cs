using CourierManagementSystem.Api.Models.DTOs.Requests;
using CourierManagementSystem.Api.Models.DTOs.Responses;

namespace CourierManagementSystem.Api.Services;

public class RouteService : IRouteService
{
    private readonly IOpenStreetMapService _openStreetMapService;

    public RouteService(IOpenStreetMapService openStreetMapService)
    {
        _openStreetMapService = openStreetMapService;
    }

    public async Task<RouteCalculationResponse> CalculateRouteAsync(RouteCalculationRequest request)
    {
        List<DeliveryPointRequest> points = request.Points;
        if (points == null || points.Count < 2)
        {
            return new RouteCalculationResponse
            {
                DistanceKm = 0,
                DurationMinutes = 0
            };
        }


        decimal totalDistance = await CalculateTotalDistanceAsync(points);
        int totalDurationMinutes = CalculateTotalDurationMinutes(totalDistance, points.Count);

        var response = new RouteCalculationResponse
        {
            DistanceKm = Math.Round(totalDistance, 2),
            DurationMinutes = totalDurationMinutes
        };

        if (totalDurationMinutes > 0)
            response.SuggestedTime = CalculateSuggestedTime(totalDurationMinutes);

        return response;
    }

    private async Task<decimal> CalculateTotalDistanceAsync(List<DeliveryPointRequest> points)
    {
        decimal totalDistance = 0;
        for (int i = 0; i < points.Count - 1; i++)
        {
            var point1 = points[i];
            var point2 = points[i + 1];

            var distance = await _openStreetMapService.CalculateDistanceAsync(
                point1.Latitude,
                point1.Longitude,
                point2.Latitude,
                point2.Longitude
            );

            totalDistance += distance;
        }
        return totalDistance;
    }

    private int CalculateTotalDurationMinutes(decimal totalDistance, int pointCount)
    {
        // Assuming average speed of 40 km/h in urban areas
        const decimal averageSpeedKmh = 40m;

        var durationHours = totalDistance / averageSpeedKmh;
        var durationMinutes = (int)Math.Ceiling(durationHours * 60);

        // Add 5 minutes per delivery point for loading/unloading
        const int minutesForLoading = 5;
        var totalDurationMinutes = durationMinutes + (pointCount * minutesForLoading);
        return totalDurationMinutes;
    }

    private SuggestedTime CalculateSuggestedTime(int totalDurationMinutes)
    {
        var startTime = TimeOnly.FromDateTime(DateTime.Now);
        var endTime = startTime.AddMinutes(totalDurationMinutes);

        return new SuggestedTime
        {
            Start = startTime,
            End = endTime
        };
    }
}
