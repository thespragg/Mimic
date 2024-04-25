using System.Text.Json;
using App.Models;

namespace App;

public static class JsonParser
{
    private static readonly JsonSerializerOptions Opts = new() { PropertyNameCaseInsensitive = true };
    
    public static IEnumerable<MockEndpoint> Parse(string json)
        => (
            JsonSerializer.Deserialize<Dictionary<string, JsonEndpoint>>(json,Opts)
            ?? new Dictionary<string, JsonEndpoint>()
        ).Select(endpoint => new MockEndpoint
        {
            Route = endpoint.Key,
            ResponseGenerator = endpoint.Value.ResponseGenerator,
            Response = endpoint.Value.Response,
            Methods = endpoint.Value.Method is not null ? [endpoint.Value.Method] : endpoint.Value.Methods ?? Enumerable.Empty<string>()
        });

    private readonly struct JsonEndpoint
    {
        public string? Method { get; init; }
        public string[]? Methods { get; init; }
        public string? Response { get; init; }
        public string? ResponseGenerator { get; init; }
    }
}