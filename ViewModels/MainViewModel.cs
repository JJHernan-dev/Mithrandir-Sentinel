using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Mithrandir_Sentinel.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private object currentView;

        public MainViewModel()
        {
            CurrentView = new DashboardViewModel();
        }

        [RelayCommand]
        private void ShowDashboard() => CurrentView = new DashboardViewModel();

        [RelayCommand]
        private void ShowConnections() => CurrentView = new ConnectionsViewModel();

        [RelayCommand]
        private void ShowAlerts() => CurrentView = new AlertsViewModel();

        [RelayCommand]
        private void ShowSettings() => CurrentView = new SettingsViewModel();

        [ObservableProperty]
        private int alerts;
    }
}
