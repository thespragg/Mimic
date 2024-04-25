using App.Configuration;
using App.Extensions;
using CommandLine;

namespace App;

public class Program
{
    public static void Main(string[] args)
    {
        var result = Parser.Default.ParseArguments<Options>(args);

        var files = result.MapResult(
            opts =>
            {
                if (opts is { File: not null, Folder: not null })
                {
                    Console.WriteLine("Error: Only one of file or folder can be set.");
                    return Enumerable.Empty<string>();
                }

                if (opts.File != null || opts.Folder != null)
                {
                    return Enumerable.Empty<string>().Concat(opts.File is not null
                        ? [opts.File]
                        : Directory.GetFiles(opts.Folder!.Replace("\"", "").Trim(), "*.json"));
                }

                Console.WriteLine("Error: Either file or folder must be set.");
                return Enumerable.Empty<string>();
            },
            errs => Enumerable.Empty<string>()).ToArray();

        if (files.Length == 0) return;

        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();
        app.UseHttpsRedirection();

        foreach (var file in files)
        {
            app.AddEndpoints(File.ReadAllText(file));
        }
        app.Run();
    }
}