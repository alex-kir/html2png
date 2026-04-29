using System.CommandLine;

namespace htmltool;

class Program
{
    static int Main(string[] args)
    {
        var o = new Options();

        var rootCommand = new RootCommand("HTML tool")
        {
            ScreenshotCommand.Create(o),
            InteractiveCommand.Create(o),
        };

        ParseResult parseResult = rootCommand.Parse(args);
        var retval = parseResult.Invoke();

        CefHelper.Shutdown();
        return retval;
    }
}

