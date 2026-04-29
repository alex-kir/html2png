using CefSharp;
using CefSharp.WinForms;
using System.CommandLine;

namespace htmltool;

class InteractiveCommand
{
    public static Command Create(Options o)
    {
        var command = new Command("interactive", "Open HTML in an interactive browser window")
        {
            o.inputOption,

            o.widthOption,
            o.heightOption,

            o.cachePathOption,
            o.cefLogFileOption,
            o.cefLogLevelOption,
        };

        command.SetAction(async parseResult =>
        {
            return await Run(new OptionsResult(o, parseResult));
        });

        return command;
    }

    static async Task<int> Run(OptionsResult o)
    {
        using var _ = await Utils.Lock(o.CachePath + ".lock");

        CefHelper.Init(o.CachePath, o.CefLogFile, o.CefLogLevel);

        await ShowAsync(o);

        return 0;
    }

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
            //    // Cancel
            //    e.Handled = true;
            //    // Load URL in current browser
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
