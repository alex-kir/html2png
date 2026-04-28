using CefSharp;
using CefSharp.Internals;
using System.Reactive.Linq;

namespace htmltool;

static class Utils
{
    public static async Task<IDisposable> Lock(string lockPath)
    {
        lockPath = Path.GetFullPath(lockPath);
        Directory.CreateDirectory(Path.GetDirectoryName(lockPath)!);

        while (true)
        {
            var fs = TryAcquireLock(lockPath);
            if (fs != null)
                return new FileLock(fs, lockPath);

            await WaitForLockRelease(lockPath);
        }
    }

    private static FileStream? TryAcquireLock(string lockPath)
    {
        try
        {
            return new FileStream(lockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        }
        catch (IOException)
        {
            return null;
        }
    }

    private static Task WaitForLockRelease(string lockPath)
    {
        var tcs = new TaskCompletionSource();
        var dir = Path.GetDirectoryName(lockPath)!;
        var file = Path.GetFileName(lockPath);

        FileSystemWatcher? watcher = null;
        try
        {
            watcher = new FileSystemWatcher(dir, file)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                EnableRaisingEvents = true,
            };

            void OnEvent(object? s, FileSystemEventArgs e)
            {
                watcher?.Dispose();
                watcher = null;
                tcs.TrySetResult();
            }

            watcher.Changed += OnEvent;
            watcher.Deleted += OnEvent;
            watcher.Created += OnEvent;
            watcher.Error += (s, e) => { watcher?.Dispose(); tcs.TrySetResult(); };
        }
        catch
        {
            watcher?.Dispose();
        }

        // Fallback timer: fires if watcher misses the event (crash, network drive, etc.)
        _ = Task.Delay(500).ContinueWith(_ => tcs.TrySetResult());

        return tcs.Task;
    }

    private sealed class FileLock(FileStream stream, string lockPath) : IDisposable
    {
        public void Dispose()
        {
            stream.Dispose();
            try { File.Delete(lockPath); } catch { }
        }
    }

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
