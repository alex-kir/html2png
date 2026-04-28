using CefSharp;
using CefSharp.OffScreen;

namespace htmltool;

static class CefHelper
{
    public static void Init(string cachePath, string logFilePath = null, LogSeverity severity = LogSeverity.Default)
    {
        if (Cef.IsInitialized == true)
            return;

        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string nativePath = Path.Combine(basePath, "runtimes", "win-x64", "native");

        var settings = new CefSettings()
        {
            BrowserSubprocessPath = Path.Combine(nativePath, "CefSharp.BrowserSubprocess.exe"),
            LocalesDirPath = Path.Combine(nativePath, "locales"),
            ResourcesDirPath = nativePath,
            CachePath = Path.GetFullPath(cachePath),

            // This will force CEF to ignore the main .exe's arguments
            // and use only those specified in CefCommandLineArgs
            CommandLineArgsDisabled = false,

            MultiThreadedMessageLoop = true,
            RemoteDebuggingPort = 0, // 0 - disable debugging
            LogFile = logFilePath,
            LogSeverity = severity,
            PersistSessionCookies = false,
        };

        // Sometimes helps in specific environments
        settings.CefCommandLineArgs.Add("no-sandbox", "1");
        // Disable hardware acceleration (critical for stable OSR)
        settings.CefCommandLineArgs.Add("disable-gpu", "1");
        settings.CefCommandLineArgs.Add("disable-gpu-compositing", "1");
        settings.CefCommandLineArgs.Add("disable-gpu-sandbox", "1");
        // Prevent GPU process from creating a visible window (happens in non-interactive sessions like Task Scheduler)
        settings.CefCommandLineArgs.Add("disable-software-rasterizer", "1");
        settings.CefCommandLineArgs.Add("in-process-gpu", "1");
        // Force software rendering
        settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling", "1");
        // Just in case, disable extensions that can open windows
        settings.CefCommandLineArgs.Add("disable-extensions", "1");
        // Run network service in-process to avoid spawning a utility subprocess window
        settings.CefCommandLineArgs.Add("disable-features", "NetworkService,NetworkServiceInProcess");


        Cef.Initialize(settings);
    }

    public static void Shutdown()
    {
        if (Cef.IsInitialized == true)
        {
            Cef.Shutdown();
        }
    }
}

