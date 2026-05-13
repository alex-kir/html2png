
using CefSharp;

namespace htmltool;

public class CustomContextMenuHandler : IContextMenuHandler
{
    private const CefMenuCommand ShowDevToolsCommand = (CefMenuCommand)CefMenuCommand.UserFirst + 1;

    public void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
    {
        if (model.Count > 0)
        {
            model.AddSeparator();
        }

        model.AddItem(ShowDevToolsCommand, "Web Developer Tools");
    }

    public bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
    {
        if (commandId == ShowDevToolsCommand)
        {
            browser.ShowDevTools();
            return true;
        }

        return false;
    }

    public void OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
    {
    }

    public bool RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
    {
        return false;
    }
}