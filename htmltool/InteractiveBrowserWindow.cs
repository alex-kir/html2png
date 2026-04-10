
using CefSharp;
using CefSharp.WinForms;

namespace htmltool
{
    public static class InteractiveBrowserForm
    {
        static internal Task<string> ShowAsync(OptionsResult o)
        {
            var _completionSource = new TaskCompletionSource<string>();

            var thread = new Thread(() =>
            {
                var _form = new Form
                {
                    Text = "Browser Preview",
                    Width = o.Width,
                    Height = o.Height,
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.Sizable,
                    MaximizeBox = true,
                    MinimizeBox = true,
                    ShowInTaskbar = true,
                    TopMost = true
                };

                var browser = new ChromiumWebBrowser(o.InputUrl)
                {
                    Dock = DockStyle.Fill,
                    LifeSpanHandler = new CustomLifeSpanHandler(),
                };

                //browser.NewWindow += (sender, e) =>
                //{
                //    // Отменяем открытие в новом окне
                //    e.Handled = true;

                //    // Загружаем URL в текущем браузере
                //    browser.Load(e.TargetUrl);
                //};

                _form.Controls.Add(browser);

                _form.ShowDialog();
                //Application.Run(_form);

                _form.Controls.Remove(browser);

                browser.Stop();
                browser.GetBrowser().StopLoad();
                browser.Dispose();

                _completionSource?.TrySetResult(browser.Address);
                _form = null;
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return _completionSource.Task;
        }
    }
}
