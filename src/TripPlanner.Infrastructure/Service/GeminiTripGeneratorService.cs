using TripPlanner.Application;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TripPlanner.Application.Interfaces;
using TripPlanner.Domain.Entities;
namespace TripPlanner.Infrastructure.Service;

public class GeminiTripGeneratorService : ITripGeneratorService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;
    public GeminiTripGeneratorService(HttpClient httpClient, IConfiguration configuration)
    {
        _apiKey = configuration["Gemini:ApiKey"];
        _httpClient = httpClient;
    }

    public async Task<TripPlan> GenerateTripAsync(string destination, int days, int guests)
    {
        // The classic standard model
        // "gemini-2.5-flash" is the current standard for speed and cost as of 2026
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";
        var prompt = $@"
                Act as a professional travel planner. Plan a trip to {destination} for {days} days for {guests} guests.
                You MUST provide exactly 3 options: 
                1. Budget Friendly (Low cost)
                2. Mid-Range (Comfort)
                3. Premium (Luxury)
                
                Return ONLY raw JSON. Do not use Markdown code blocks.
                Follow this exact schema:
                {{
                    ""destination"": ""{destination}"",
                    ""durationDays"": {days},
                    ""numberOfGuests"": {guests},
                    ""options"": [
                        {{
                            ""tier"": ""Budget"",
                            ""estimatedTotalCost"": 500,
                            ""currency"": ""USD"",
                            ""vibe"": ""Backpacker"",
                            ""itinerary"": [
                                {{ ""day"": 1, ""morningActivity"": ""..."", ""afternoonActivity"": ""..."", ""eveningActivity"": ""..."" }}
                            ]
                        }}
                    ]
         }}";
        var requestBody = new
        {
            contents = new[] { new { parts = new[] { new { text = prompt } } } },
            generationConfig = new { response_mime_type = "application/json" }
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        // 4. Send to Google
        var response = await _httpClient.PostAsync(url, jsonContent);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        // 5. Parse the Response
        return ParseGeminiResponse(responseString);
    }
    private TripPlan ParseGeminiResponse(string rawJson)
    {
        using var doc = JsonDocument.Parse(rawJson);
            
        // Navigate Gemini's deep JSON structure
        var text = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        // Clean up if Gemini accidentally added markdown
        text = text.Replace("```json", "").Replace("```", "").Trim();

        // Convert to our Domain Entity
        return JsonSerializer.Deserialize<TripPlan>(text, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}