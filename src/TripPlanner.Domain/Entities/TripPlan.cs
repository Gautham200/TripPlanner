namespace TripPlanner.Domain.Entities;

public class TripPlan
{
    public string Destination { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public int NumberOfGuests { get; set; }
        
    // This list will hold exactly 3 items: Budget, Mid, Premium
    public List<TripOption> Options { get; set; } = new();
}