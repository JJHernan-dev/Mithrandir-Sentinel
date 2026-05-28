using System.Windows.Controls;
using Mithrandir_Sentinel.ViewModels;

namespace Mithrandir_Sentinel.Views
{
    public partial class AlertsView : UserControl
    {
        public AlertsView()
        {
            InitializeComponent();

            DataContext = new AlertsViewModel();
        }
    }
}
