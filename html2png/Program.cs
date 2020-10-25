using CefSharp;
using CefSharp.OffScreen;
using Services;
using System;
using System.Drawing;
using System.Reactive.Linq;
using System.Threading.Tasks;

// https://groups.google.com/g/cefglue/c/mMpJCcftfQU?pli=1
// https://stackoverflow.com/questions/43461640/wait-for-a-page-to-load-with-cefsharp

namespace html2png
{
    [AppOptions(Description = "Tool renders HTML into PNG")]
    class Options
    {
        [AppOptions(FullKeys = new[] { "help" }, ShortKeys = new[] { "h" })]
        public bool Help { get; set; }

        [AppOptions(FullKeys = new[] { "input" }, ShortKeys = new[] { "i" }, Description = "Input HTML url.")]
        public string InputFile { get; set; }

        [AppOptions(FullKeys = new[] { "output" }, ShortKeys = new[] { "o" }, Description = "Output PNG filename.")]
        public string OutputFile { get; set; }

        [AppOptions(FullKeys = new[] { "width" }, ShortKeys = new[] { "w" }, Description = "Browser window width.")]
        public int Width { get; set; } = 1024;

        [AppOptions(FullKeys = new[] { "height" }, ShortKeys = new[] { "h" }, Description = "Browser window height.")]
        public int Height { get; set; } = 768;

        [AppOptions(FullKeys = new[] { "dpi" }, Description = "Browser window DPI.")]
        public int Dpi { get; set; } = 192;

        [AppOptions(FullKeys = new[] { "cache-path" }, Description = "Cache path.")]
        public string CachePath { get; set; }

        [AppOptions(FullKeys = new[] { "delay" }, Description = "Delay, seconds.")]
        public double DelaySeconds { get; set; } = 1;
    }

    class Program
    {
        private static bool Validate(string[] args, Options o)
        {
            if (!AppOptions.TryParse(args, o))
                return false;

            if (o.Help)
                return false;

            if (string.IsNullOrWhiteSpace(o.InputFile))
                return false;

            if (string.IsNullOrWhiteSpace(o.OutputFile))
                return false;

            return true;
        }

        static void Main(string[] args)
        {
            var o = new Options();
            if (!Validate(args, o))
            {
                AppOptions.PrintHelp<Options>();
                return;
            }

            var settings = new CefSettings()
            {
                CachePath = o.CachePath,
            };

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            MainAsync(o).Wait();

            Cef.Shutdown();
        }

        static async Task MainAsync(Options o)
        {
            var browser = new ChromiumWebBrowser();

            browser.LoadingStateChanged += (s, e) => {
                Console.WriteLine($"IsLoading:{browser.IsLoading}");
            };

            await Observable.FromEventPattern(
                h => browser.BrowserInitialized += h,
                h => browser.BrowserInitialized -= h)
                .Select(it => browser.IsBrowserInitialized)
                .StartWith(browser.IsBrowserInitialized)
                .FirstAsync(it => it);

            browser.Size = new Size(o.Width, o.Height);
            browser.Load(o.InputFile);

            //await WaitLoading(browser);
            if (o.DelaySeconds > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(o.DelaySeconds));
            }
            await WaitLoading(browser);
            var zoom = await browser.GetZoomLevelAsync();
            //browser.SetZoomLevel(0.5);

            var screenshot = await browser.ScreenshotAsync();
            screenshot.Save(o.OutputFile);
        }

        private static async Task WaitLoading(ChromiumWebBrowser browser)
        {
            await Observable.FromEventPattern<LoadingStateChangedEventArgs>(h => browser.LoadingStateChanged += h, h => browser.LoadingStateChanged -= h)
                .Select(it => it.EventArgs.IsLoading)
                .StartWith(browser.IsLoading)
                .FirstAsync(it => !it);
        }
    }
}
