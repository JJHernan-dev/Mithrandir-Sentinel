using System.Collections.ObjectModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Mithrandir_Sentinel.Core;
using Mithrandir_Sentinel.Models;
using Mithrandir_Sentinel.Services;
using static Mithrandir_Sentinel.ViewModels.SettingsViewModel;

namespace Mithrandir_Sentinel.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly DispatcherTimer _timer;
        private readonly NetworkService _networkService;
        private bool _demoMode;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private int activeConnections;

        [ObservableProperty]
        private int alerts;

        [ObservableProperty]
        private ObservableCollection<ConnectionInfo> connections = new();

        [ObservableProperty]
        private int monitoredProcesses;

        [ObservableProperty]
        private int highRiskConnections;

        [ObservableProperty]
        private ObservableCollection<ConnectionInfo> recentHighRiskConnections = new();

        public DashboardViewModel()
        {
            WeakReferenceMessenger.Default.Register<RefreshIntervalChangedMessage>(
                this,
                (r, m) =>
                {
                    _timer.Interval = TimeSpan.FromSeconds(m.Value);
                }
            );

            _demoMode = Properties.Settings.Default.EnableThreatDetection;

            _networkService = new NetworkService();

            _threatDetectionService = new ThreatDetectionService();

            _timer = new DispatcherTimer();

            _timer.Interval = TimeSpan.FromSeconds(Properties.Settings.Default.RefreshInterval);

            _timer.Tick += UpdateMetrics;

            _timer.Start();

            _ = LoadDashboardAsync();
        }

        private void UpdateMetrics(object? sender, EventArgs e)
        {
            var activeConnections = _networkService.GetActiveTcpConnections();

            if (_demoMode)
            {
                var random = new Random();

                foreach (var c in activeConnections)
                {
                    c.RiskLevel = random.Next(0, 10) > 7 ? "High" : "Low";
                }
            }

            ConnectionRepository.Connections.Clear();

            foreach (var connection in activeConnections)
            {
                ConnectionRepository.Connections.Add(connection);
            }

            var detectedAlerts = _threatDetectionService.AnalyzeConnections(activeConnections);

            SecurityAlerts.Clear();

            foreach (var alert in detectedAlerts)
            {
                SecurityAlerts.Add(alert);
            }

            ActiveConnections = activeConnections.Count;

            Alerts = detectedAlerts.Count;

            HighRiskConnections = activeConnections.Count(c => c.RiskLevel == "High");

            MonitoredProcesses = activeConnections.Select(c => c.ProcessName).Distinct().Count();
            RecentHighRiskConnections.Clear();

            var highRiskConnections = activeConnections.Where(c => c.RiskLevel == "High").Take(5);

            foreach (var connection in highRiskConnections)
            {
                RecentHighRiskConnections.Add(connection);
            }

            OnPropertyChanged(nameof(HasHighRiskConnections));
        }

        [ObservableProperty]
        private ObservableCollection<SecurityAlert> securityAlerts = new();

        private readonly ThreatDetectionService _threatDetectionService;

        public bool HasHighRiskConnections => RecentHighRiskConnections?.Any() == true;

        private async Task LoadDashboardAsync()
        {
            IsLoading = true;

            await Task.Delay(Properties.Settings.Default.RefreshInterval * 1000);

            UpdateMetrics(null, EventArgs.Empty);

            IsLoading = false;
        }
    }
}
