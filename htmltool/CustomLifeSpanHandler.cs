
#if true
using CefSharp;
//using System.Windows.fo;

namespace htmltool
{
    public class CustomLifeSpanHandler : ILifeSpanHandler
    {
        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
            string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition,
            bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo,
            IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            browser.MainFrame.LoadUrl(targetUrl);
            newBrowser = null;
            return true; // true = отмена стандартного попапа
        }

        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser) => false;
        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser) { }
        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser) { }
    }
}


// Обновленный метод MainAsync
//static async Task<int> MainAsync(Options o)
//        {
//            var browser = new ChromiumWebBrowser();

//            browser.LoadingStateChanged += (s, e) =>
//            {
//                Console.WriteLine($"IsLoading:{browser.IsLoading}");
//            };

//            await Observable.FromEventPattern(
//                h => browser.BrowserInitialized += h,
//                h => browser.BrowserInitialized -= h)
//                .Select(it => browser.IsBrowserInitialized)
//                .StartWith(browser.IsBrowserInitialized)
//                .FirstAsync(it => it);

//            browser.Size = new Size(o.Width, o.Height);
//            browser.Load(o.InputFile);

//            await WaitLoading(browser);

//            if (o.DelaySeconds > 0)
//            {
//                Console.WriteLine("Открывается интерактивное окно. Взаимодействуйте со страницей и закройте окно для продолжения...");

//                // Создаем и показываем интерактивное окно
//                using (var interactiveForm = new InteractiveBrowserForm(browser, o.Width, o.Height))
//                {
//                    await interactiveForm.ShowAsync();
//                }

//                Console.WriteLine("Окно закрыто, продолжаем...");

//                // Даем время на стабилизацию после закрытия окна
//                await Task.Delay(500);
//            }

//            var screenshot = await browser.ScreenshotAsync();
//            screenshot.Save(o.OutputFile);

//            _processing = false;

//            return 0;
//        }
//    }
//}

#endif
