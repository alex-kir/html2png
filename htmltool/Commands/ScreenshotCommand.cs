using CefSharp;
using CefSharp.OffScreen;
using System.CommandLine;
using System.Text.Json;

namespace htmltool;

class ScreenshotCommand
{
    public static Command Create(Options o)
    {
        var command = new Command("screenshot", "Render HTML into PNG")
        {
            o.inputOption,
            o.outputOption,
            o.cachePathOption,

            o.widthOption,
            o.heightOption,
            o.dpiOption,
            o.delayOption,

            o.xpathOption,

            o.cefLogFileOption,
            o.cefLogLevelOption,
        };

        command.SetAction(async parseResult =>
        {
            return await Run(new OptionsResult(o, parseResult));
        });

        return command;
    }

    static async Task<int> Run(OptionsResult o)
    {
        using var _ = await Utils.Lock(o.CachePath + ".lock");

        CefHelper.Init(o.CachePath, o.CefLogFile, o.CefLogLevel);

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
        browser.Load(o.InputUrl);

        await Utils.WhenLoadingCompleted(browser);

        if (o.DelayMilliseconds > 0)
            await Task.Delay(TimeSpan.FromMilliseconds(o.DelayMilliseconds));

        byte[] screenshot;

        if (o.XPath is not null)
        {
            var xpathJson = JsonSerializer.Serialize(o.XPath);
            var script = $@"(function() {{
                var el = document.evaluate({xpathJson}, document, null,
                    XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;
                if (!el) return null;
                var r = el.getBoundingClientRect();
                return {{ x: r.left, y: r.top, width: r.width, height: r.height }};
            }})()";

            var jsResponse = await browser.EvaluateScriptAsync(script);
            if (!jsResponse.Success || jsResponse.Result is null)
            {
                Console.Error.WriteLine($"Element not found by xpath: {o.XPath}");
                browser.Stop();
                browser.GetBrowser().StopLoad();
                return 2;
            }

            var rect = (IDictionary<string, object>)jsResponse.Result;
            var clip = new CefSharp.DevTools.Page.Viewport
            {
                X = Convert.ToDouble(rect["x"]),
                Y = Convert.ToDouble(rect["y"]),
                Width = Convert.ToDouble(rect["width"]),
                Height = Convert.ToDouble(rect["height"]),
                Scale = 1,
            };

            var response = await browser.GetDevToolsClient().Page.CaptureScreenshotAsync(
                format: CefSharp.DevTools.Page.CaptureScreenshotFormat.Png,
                clip: clip);
            screenshot = response.Data;
        }
        else
        {
            // TODO use dpi
            screenshot = await browser.CaptureScreenshotAsync(format: CefSharp.DevTools.Page.CaptureScreenshotFormat.Png);
        }

        File.WriteAllBytes(o.OutputFile, screenshot);

        browser.Stop();
        browser.GetBrowser().StopLoad();

        return 0;
    }
}
