using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Mithrandir_Sentinel.Messages;
using Mithrandir_Sentinel.Models;
using Mithrandir_Sentinel.Services;

namespace Mithrandir_Sentinel.ViewModels
{
    public partial class ConnectionsViewModel : ObservableObject
    {
        private readonly DispatcherTimer _timer;
        private readonly NetworkService _networkService;

        [ObservableProperty]
        private ObservableCollection<ConnectionInfo> connections = new();

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private bool isLoading;

        public ConnectionsViewModel()
        {
            WeakReferenceMessenger.Default.Register<RefreshIntervalChangedMessage>(
                this,
                (r, m) =>
                {
                    _timer.Interval = TimeSpan.FromSeconds(m.Value);
                }
            );

            _networkService = new NetworkService();

            FilteredConnections = CollectionViewSource.GetDefaultView(Connections);

            FilteredConnections.Filter = FilterConnections;

            _timer = new DispatcherTimer();

            _timer.Interval = TimeSpan.FromSeconds(Properties.Settings.Default.RefreshInterval);

            _timer.Tick += UpdateConnections;

            _timer.Start();

            _ = LoadConnectionsAsync();
        }

        private void UpdateConnections(object? sender, EventArgs e)
        {
            _timer.Interval = TimeSpan.FromSeconds(Properties.Settings.Default.RefreshInterval);

            var activeConnections = _networkService.GetActiveTcpConnections();

            if (Properties.Settings.Default.EnableThreatDetection)
            {
                var random = new Random();

                foreach (var c in activeConnections)
                {
                    c.RiskLevel = random.Next(0, 10) > 7 ? "High" : "Low";
                }
            }

            Connections.Clear();

            foreach (var connection in activeConnections)
            {
                Connections.Add(connection);
            }

            FilteredConnections.Refresh();
        }

        public ICollectionView FilteredConnections { get; }

        private bool FilterConnections(object obj)
        {
            if (obj is not ConnectionInfo connection)
                return false;

            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            string search = SearchText.ToLower();

            return connection.ProcessName.ToLower().Contains(search)
                || connection.LocalAddress.ToLower().Contains(search)
                || connection.RemoteAddress.ToLower().Contains(search)
                || connection.State.ToLower().Contains(search);
        }

        partial void OnSearchTextChanged(string value)
        {
            FilteredConnections.Refresh();
        }

        private async Task LoadConnectionsAsync()
        {
            IsLoading = true;

            await Task.Delay(Properties.Settings.Default.RefreshInterval * 1000);

            UpdateConnections(null, EventArgs.Empty);

            IsLoading = false;
        }
    }
}
