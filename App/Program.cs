using App.Extensions;

namespace App;

public class Program
{
    public static void Main(string[] args)
    {
        const string json = "{\"/health\":{\"method\":\"GET\",\"response\":\"{\\u0022status\\u0022:\\u0022healthy\\u0022}\"}}";
        
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();
        app.UseHttpsRedirection(); 
        app.AddEndpoints(json);
        app.Run();
    }
}