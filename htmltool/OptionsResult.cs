using CefSharp;
using System.CommandLine;

namespace htmltool;

class OptionsResult(Options options, ParseResult parseResult)
{
    private readonly Options _opts = options;
    private readonly ParseResult _parse = parseResult;

    public bool InteractiveMode => _parse.GetValue(_opts.interactiveOption);

    public string InputUrl => _parse.GetValue(_opts.inputOption)!;
    public string OutputFile => _parse.GetValue(_opts.outputOption)!;
    public string CachePath => _parse.GetValue(_opts.cachePathOption)!;

    public int Width => _parse.GetValue(_opts.widthOption);
    public int Height => _parse.GetValue(_opts.heightOption);
    public int DelayMilliseconds => _parse.GetValue(_opts.delayOption);

    public string? CefLogFile => _parse.GetValue(_opts.cefLogFileOption);
    public LogSeverity CefLogLevel => _parse.GetValue(_opts.cefLogLevelOption);

    public string? XPath => _parse.GetValue(_opts.xpathOption);
}

