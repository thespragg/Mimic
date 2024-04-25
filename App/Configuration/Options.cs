using CommandLine;

namespace App.Configuration;

public class Options
{
    [Option('f', "file", Required = false, HelpText = "File containing the endpoint JSON data.")]
    public string? File { get; set; }

    [Option('d', "folder", Required = false, HelpText = "Folder containing JSON files for mimicking.")]
    public string? Folder { get; set; }
}