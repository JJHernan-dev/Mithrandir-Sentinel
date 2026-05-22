using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Threading;
using Mithrandir_Sentinel.Services;
using System.Collections.ObjectModel;
using Mithrandir_Sentinel.Models;

namespace Mithrandir_Sentinel.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly DispatcherTimer _timer;
        private readonly NetworkService _networkService;

        [ObservableProperty]
        private int activeConnections = 12;

        [ObservableProperty]
        private int alerts = 12;

        [ObservableProperty]
        private ObservableCollection<ConnectionInfo> connections = new();

        public DashboardViewModel()

        {
            _networkService = new NetworkService();

            _timer = new DispatcherTimer();

            _timer.Interval = TimeSpan.FromSeconds(2);

            _timer.Tick += UpdateMetrics;

            _timer.Start();
        }

        private void UpdateMetrics(Object? senser, EventArgs e)
        {
            var activeConnections = _networkService.GetActiveTcpConnections();

            Connections.Clear();

            foreach (var connection in activeConnections)
            {
                Connections.Add(connection);
            }

            ActiveConnections = activeConnections.Count;

            Alerts = 0;
        }
    }
}