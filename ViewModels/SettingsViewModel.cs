using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Mithrandir_Sentinel.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool enableDemoAlerts;

        [ObservableProperty]
        private bool enableThreatDetection;

        [ObservableProperty]
        private int refreshInterval;

        public SettingsViewModel()
        {
            EnableDemoAlerts = Properties.Settings.Default.EnableThreatDetection;

            EnableThreatDetection = Properties.Settings.Default.EnableThreatDetection;

            RefreshInterval = Properties.Settings.Default.RefreshInterval;
        }

        partial void OnEnableDemoAlertsChanged(bool value)
        {
            Properties.Settings.Default.EnableThreatDetection = value;
            Properties.Settings.Default.Save();
        }

        partial void OnEnableThreatDetectionChanged(bool value)
        {
            Properties.Settings.Default.EnableThreatDetection = value;
            Properties.Settings.Default.Save();
        }

        partial void OnRefreshIntervalChanged(int value)
        {
            Properties.Settings.Default.RefreshInterval = value;
            Properties.Settings.Default.Save();

            WeakReferenceMessenger.Default.Send(new RefreshIntervalChangedMessage(value));
        }

        public class RefreshIntervalChangedMessage
        {
            public int Value { get; }

            public RefreshIntervalChangedMessage(int value)
            {
                Value = value;
            }
        }
    }
}
