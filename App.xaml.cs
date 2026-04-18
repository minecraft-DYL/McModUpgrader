using McModUpgrader.Services;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Navigation;

namespace McModUpgrader
{
    public partial class App : Application
    {
        private Window? _window;

        // 全局窗口访问（用于文件选择器 InitializeWithWindow）
        public static Window? CurrentWindow { get; private set; }

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            _window ??= new Window();
            CurrentWindow = _window;

            // 设置窗口大小
            _window.AppWindow.Resize(new Windows.Graphics.SizeInt32(960, 680));
            _window.Title = "Minecraft Mod 升级器";
            _window.ExtendsContentIntoTitleBar = false;

            if (_window.Content is not Frame rootFrame)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                _window.Content = rootFrame;
            }

            _ = rootFrame.Navigate(typeof(Views.MainPage), e.Arguments);
            _window.Activate();

            // 窗口关闭时清理残留的 Temp 目录（中途关闭程序的情况）
            _window.Closed += (_, _) => UpgradeOrchestratorService.CleanupTempDir();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
    }
}
