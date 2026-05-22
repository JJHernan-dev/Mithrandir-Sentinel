using System.Windows.Controls;
using Mithrandir_Sentinel.ViewModels;

namespace Mithrandir_Sentinel.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();

            DataContext = new DashboardViewModel();
        }
    }
}