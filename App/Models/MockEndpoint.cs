namespace App.Models;

public readonly struct MockEndpoint
{
    public string Method { get; init; }
    public string Route { get; init; }
    public string? Response { get; init; }
    public string? ResponseGenerator { get; init; }
}