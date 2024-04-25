using System.Text.Json;
using App.Models;

namespace App;

public static class JsonParser
{
    public static IEnumerable<MockEndpoint> Parse(string json)
        => (
            JsonSerializer.Deserialize<Dictionary<string, JsonEndpoint>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? new Dictionary<string, JsonEndpoint>()
        ).Select(endpoint => new MockEndpoint
        {
            Route = endpoint.Key,
            ResponseGenerator = endpoint.Value.ResponseGenerator,
            Response = endpoint.Value.Response,
            Method = endpoint.Value.Method
        });

    private readonly struct JsonEndpoint
    {
        public required string Method { get; init; }
        public string? Response { get; init; }
        public string? ResponseGenerator { get; init; }
    }
}