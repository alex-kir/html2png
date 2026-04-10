//using Microsoft.VisualBasic.FileIO;
using CefSharp;
using System.CommandLine;
//using static System.Windows.Forms.Design.AxImporter;

// https://groups.google.com/g/cefglue/c/mMpJCcftfQU?pli=1
// https://stackoverflow.com/questions/43461640/wait-for-a-page-to-load-with-cefsharp

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
}

