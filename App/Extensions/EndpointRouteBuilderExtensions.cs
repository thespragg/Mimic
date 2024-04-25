using System.Text.Json;
using App.Models;

namespace App.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void AddEndpoints(
        this IEndpointRouteBuilder builder,
        string config
    )
    {
        var endpoints = JsonParser.Parse(config);
        foreach (var endpoint in endpoints)
        {
            builder.MapMethods(
                endpoint.Route,
                endpoint.Methods,
                () => JsonDocument.Parse(endpoint.Response ?? ResponseFactory.Create(endpoint.ResponseGenerator))
            );
        }
    }
}