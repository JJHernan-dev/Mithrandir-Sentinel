using System.Collections.ObjectModel;
using Mithrandir_Sentinel.Models;

namespace Mithrandir_Sentinel.Core
{
    public static class ConnectionRepository
    {
        public static ObservableCollection<ConnectionInfo> Connections { get; } = new();
    }
}
