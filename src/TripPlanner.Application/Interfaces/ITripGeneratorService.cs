namespace TripPlanner.Application.Interfaces;
using TripPlanner.Domain.Entities;
public interface ITripGeneratorService
{
    // The contract: Give me destination details, and I promise to return a TripPlan.
    Task<TripPlan> GenerateTripAsync(string destination, int days, int guests);
}