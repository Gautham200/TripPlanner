namespace TripPlanner.Application.DTOs;

public class TripRequestDto
{
    public string Destination { get; set; } = string.Empty;
    public int NumberOfDays { get; set; }
    public int NumberOfGuests { get; set; }
}