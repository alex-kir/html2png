using CefSharp;
using CefSharp.Internals;
using System.Reactive.Linq;

namespace htmltool;

static class Utils
{
    public static async Task WhenInitialized(CefSharp.OffScreen.ChromiumWebBrowser browser)
    {
        browser.FrameLoadStart += (s, aa) =>
        {
            Console.WriteLine($"FrameLoadStart: {aa.Frame.IsMain} - {aa.Frame.Name}");
        };

        browser.FrameLoadEnd += (s, aa) =>
        {
            Console.WriteLine($"FrameLoadEnd: {aa.Frame.IsMain} - {aa.Frame.Name}");
        };

        await Observable.FromEventPattern(
            h => browser.BrowserInitialized += h,
            h => browser.BrowserInitialized -= h)
            .Select(it => browser.IsBrowserInitialized)
            .StartWith(browser.IsBrowserInitialized)
            .FirstAsync(it => it);
    }

    public static async Task WhenLoadingCompleted(IRenderWebBrowser browser)
    {
        await Observable.FromEventPattern<LoadingStateChangedEventArgs>(
            h => browser.LoadingStateChanged += h,
            h => browser.LoadingStateChanged -= h)
            .Select(it => it.EventArgs.IsLoading)
            .StartWith(browser.IsLoading)
            .FirstAsync(it => !it);
    }
}

