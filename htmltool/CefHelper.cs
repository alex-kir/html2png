using CefSharp;
using CefSharp.OffScreen;

// https://groups.google.com/g/cefglue/c/mMpJCcftfQU?pli=1
// https://stackoverflow.com/questions/43461640/wait-for-a-page-to-load-with-cefsharp
// Release\net9.0-windows\runtimes\win-x64\native\CefSharp.BrowserSubprocess.exe 
// https://learn.microsoft.com/en-us/dotnet/standard/commandline/get-started-tutorial

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
            //MultiThreadedMessageLoop = false, // AI: stop open browser window
            //ExternalMessagePump = false, // AI: stop open browser window
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

