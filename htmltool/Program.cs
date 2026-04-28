using CefSharp;
using CefSharp.OffScreen;
using System.CommandLine;

namespace htmltool;

class Program
{
    static int Main(string[] args)
    {
        Console.WriteLine($"ARGS: {string.Join(", ", args.Select(a => $"'{a}'"))}");

        // Safety: if somehow launched as a CEF browser subprocess, exit immediately
        // if (args.Any(a => a.StartsWith("--type=")))
        //     return 0;

        var o = new Options();

        var screenshotCommand = new Command("screenshot", "Render HTML into PNG")
        {
            o.interactiveOption,

            o.inputOption,
            o.outputOption,
            o.cachePathOption,

            o.widthOption,
            o.heightOption,
            o.dpiOption,
            o.delayOption,

            o.cefLogFileOption,
            o.cefLogLevelOption,
        };

        screenshotCommand.SetAction(async parseResult =>
        {
            return await OnRenderCommand(new OptionsResult(o, parseResult));
        });

        var rootCommand = new RootCommand("HTML tool");
        rootCommand.Add(screenshotCommand);

        ParseResult parseResult = rootCommand.Parse(args);
        var retval = parseResult.Invoke();

        CefHelper.Shutdown();
        return retval;
    }

    static async Task<int> OnRenderCommand(OptionsResult o)
    {
        using var _ = await Utils.Lock(o.CachePath + ".lock");

        CefHelper.Init(o.CachePath, o.CefLogFile, o.CefLogLevel);

        var address = o.InputUrl;
        //if (o.InteractiveMode)
        //{
        //    address = await InteractiveBrowserForm.ShowAsync(o);
        //}

        using var browser = new ChromiumWebBrowser
        {
            LifeSpanHandler = new CustomLifeSpanHandler(),
            JsDialogHandler = new CustomJsDialogHandler(),
        };

        browser.LoadingStateChanged += (s, e) =>
        {
            Console.WriteLine($"IsLoading:{browser.IsLoading}");
        };

        browser.Size = new Size(o.Width, o.Height);
        browser.Load(address);

        //await Utils.WhenInitialized(browser);
        await Utils.WhenLoadingCompleted(browser);

        if (o.DelayMilliseconds > 0)
            await Task.Delay(TimeSpan.FromMilliseconds(o.DelayMilliseconds));

        // TODO use dpi;
        var screenshot = await browser.CaptureScreenshotAsync(format: CefSharp.DevTools.Page.CaptureScreenshotFormat.Png);
        File.WriteAllBytes(o.OutputFile, screenshot);

        browser.Stop();
        browser.GetBrowser().StopLoad();

        return 0;
    }
}

