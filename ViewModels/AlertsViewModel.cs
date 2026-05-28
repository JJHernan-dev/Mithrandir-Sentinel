using System.Collections.ObjectModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Mithrandir_Sentinel.Models;
using Mithrandir_Sentinel.Services;

namespace Mithrandir_Sentinel.ViewModels
{
    public partial class AlertsViewModel : ObservableObject
    {
        private readonly NetworkService _networkService;

        private readonly ThreatDetectionService _threatDetectionService;

        private readonly DispatcherTimer _timer;

        [ObservableProperty]
        private ObservableCollection<SecurityAlert> alerts = new();

        private bool _demoMode;

        [ObservableProperty]
        private bool isLoading;

        public AlertsViewModel()
        {
            _demoMode = Properties.Settings.Default.EnableThreatDetection;

            _networkService = new NetworkService();

            _threatDetectionService = new ThreatDetectionService();

            _timer = new DispatcherTimer();

            _timer.Interval = TimeSpan.FromSeconds(Properties.Settings.Default.RefreshInterval);

            _timer.Tick += UpdateAlerts;

            _timer.Start();

            _ = LoadAlertsAsync();
        }

        private void UpdateAlerts(object? sender, EventArgs e)
        {
            var connections = _networkService.GetActiveTcpConnections();

            if (_demoMode)
            {
                var random = new Random();

                foreach (var c in connections)
                {
                    c.RiskLevel = random.Next(0, 10) > 7 ? "High" : "Low";
                }
            }

            var detectedAlerts = _threatDetectionService.AnalyzeConnections(connections);

            Alerts.Clear();

            foreach (var alert in detectedAlerts)
            {
                Alerts.Add(alert);
            }

            OnPropertyChanged(nameof(HasAlerts));
        }

        public bool HasAlerts => Alerts.Any();

        private async Task LoadAlertsAsync()
        {
            IsLoading = true;

            await Task.Delay(Properties.Settings.Default.RefreshInterval * 1000);

            UpdateAlerts(null, EventArgs.Empty);

            IsLoading = false;
        }
    }
}
