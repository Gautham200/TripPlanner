using Microsoft.AspNetCore.Mvc;
using TripPlanner.Application.Interfaces;
using TripPlanner.Domain.Entities;
using TripPlanner.Application.DTOs;
namespace Trial.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripGeneratorService _tripService;

    public TripsController(ITripGeneratorService tripService)
    {
        _tripService = tripService;
    }

    [HttpPost("generate")]
    public async Task<ActionResult<TripPlan>> GenerateTrip([FromBody] TripRequestDto request)
    {
        if (request.NumberOfDays > 7)
            return BadRequest("For MVP, trips are limited to 7 days.");

        try
        {
            var trip = await _tripService.GenerateTripAsync(request.Destination, request.NumberOfDays, request.NumberOfGuests);
            return Ok(trip);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}