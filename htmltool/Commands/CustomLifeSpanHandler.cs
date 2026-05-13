
using CefSharp;

namespace htmltool;

public class CustomLifeSpanHandler : ILifeSpanHandler
{
    public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
        string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition,
        bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo,
        IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
    {
        browser.MainFrame.LoadUrl(targetUrl);
        newBrowser = null;
        return true; // disable default popup
    }

    public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser) => false;
    public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser) { }
    public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser) { }
}
