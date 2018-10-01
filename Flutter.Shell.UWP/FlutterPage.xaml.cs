using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Flutter.Shell.UWP
{
    /// <summary>
    /// Flutter Page
    /// </summary>
    public partial class FlutterPage
    {
        public FlutterPage()
        {
            InitializeComponent();
            HideTitleBar();
        }

        private static void HideTitleBar()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ForegroundColor         = Colors.White;
            titleBar.BackgroundColor         = Colors.Transparent;
            titleBar.ButtonForegroundColor   = Colors.White;
            titleBar.ButtonBackgroundColor   = Colors.Transparent;
            titleBar.InactiveBackgroundColor = Colors.Transparent;
        }
    }
}
