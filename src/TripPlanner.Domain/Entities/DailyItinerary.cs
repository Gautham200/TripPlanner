namespace TripPlanner.Domain.Entities;

public class DailyItinerary
{
    public int Day { get; set; }
    public string MorningActivity { get; set; } = string.Empty;
    public string AfternoonActivity { get; set; } = string.Empty;
    public string EveningActivity { get; set; } = string.Empty;
}