using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace Mithrandir_Sentinel.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });

            e.Handled = true;
        }

        private void GitHubButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(
                new ProcessStartInfo("https://github.com/JJHernan-dev") { UseShellExecute = true }
            );
        }

        private void LinkedInButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(
                new ProcessStartInfo("https://www.linkedin.com/in/juanjesus-gh/")
                {
                    UseShellExecute = true,
                }
            );
        }

        private void WebsiteButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(
                new ProcessStartInfo("https://jjhernan-dev.github.io/projects/mithrandir-sentinel/")
                {
                    UseShellExecute = true,
                }
            );
        }

        private void PortfolioButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(
                new ProcessStartInfo("https://jjhernan-dev.github.io/") { UseShellExecute = true }
            );
        }
    }
}
