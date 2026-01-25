namespace TripPlanner.Domain.Entities;

// Represents one of the 3 tiers (Budget, Mid, Premium)
public class TripOption
{
    public string Tier { get; set; } = string.Empty; // e.g., "Budget Friendly"
    public decimal EstimatedTotalCost { get; set; }
    public string Currency { get; set; } = "USD";
    public string Vibe { get; set; } = string.Empty; // e.g., "Luxury & Relaxation"
        
    public List<DailyItinerary> Itinerary { get; set; } = new();
}