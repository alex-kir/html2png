
using CefSharp;

namespace htmltool;

public class CustomJsDialogHandler : IJsDialogHandler
{
    public bool OnJSDialog(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
    {
        suppressMessage = true; // suppress all JS alerts
        return false;
    }
    public bool OnBeforeUnloadDialog(IWebBrowser browserControl, IBrowser browser, string messageText, bool isReload, IJsDialogCallback callback) => true;
    public void OnResetDialogState(IWebBrowser browserControl, IBrowser browser) { }
    public void OnDialogClosed(IWebBrowser browserControl, IBrowser browser) { }
}
