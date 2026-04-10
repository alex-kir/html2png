using CefSharp;
using System.CommandLine;

namespace htmltool;

class Options
{
    public readonly Option<string> inputOption = new Option<string>("--input", "-i")
    {
        Description = "Input HTML url",
        Required = true,
    };

    public readonly Option<string> outputOption = new Option<string>("--output", "-o")
    {
        Description = "Output PNG filename",
        //Required = true,
        DefaultValueFactory = _ => "out.png",
    };

    public readonly Option<int> widthOption = new Option<int>("--width")
    {
        Description = "Browser window width",
        DefaultValueFactory = _ => 1366,
    };

    public readonly Option<int> heightOption = new Option<int>("--height")
    {
        Description = "Browser window height",
        DefaultValueFactory = _ => 768,
    };

    public readonly Option<int> dpiOption = new Option<int>("--dpi")
    {
        Description = "Browser window DPI",
        DefaultValueFactory = _ => 192,
    };

    public readonly Option<string> cachePathOption = new Option<string>("--cache-path")
    {
        Description = "Cache path",
        DefaultValueFactory = _ => "htmltool_cache",
    };

    public readonly Option<int> delayOption = new Option<int>("--delay")
    {
        Description = "Delay, milliseconds",
        DefaultValueFactory = _ => 1000,
    };

    public readonly Option<bool> interactiveOption = new Option<bool>("--interactive")
    {
        Description = "Interactive mode",
        DefaultValueFactory = _ => false
    };

    public readonly Option<LogSeverity> cefLogLevelOption = new Option<LogSeverity>("--cef-log-level")
    {
        Description = "CEF log level",
        DefaultValueFactory = _ => LogSeverity.Default
    };

    public readonly Option<string> cefLogFileOption = new Option<string>("--cef-log-file")
    {
        Description = "CEF log file",
        DefaultValueFactory = _ => null,
    };
}

