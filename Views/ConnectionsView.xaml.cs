using System.Windows.Controls;
using Mithrandir_Sentinel.ViewModels;

namespace Mithrandir_Sentinel.Views
{
    public partial class ConnectionsView : UserControl
    {
        public ConnectionsView()
        {
            InitializeComponent();

            DataContext = new ConnectionsViewModel();
        }
    }
}
