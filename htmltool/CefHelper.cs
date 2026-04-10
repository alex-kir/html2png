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
            CommandLineArgsDisabled = true,
            MultiThreadedMessageLoop = true,
            RemoteDebuggingPort = 0, // 0 - disable debugging
            LogFile = logFilePath,
            LogSeverity = severity,
            PersistSessionCookies = false,
        };

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

