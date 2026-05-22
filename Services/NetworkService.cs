using Mithrandir_Sentinel.Models;
using System.Net.NetworkInformation;

namespace Mithrandir_Sentinel.Services
{
    public class NetworkService
    {
        public List<ConnectionInfo> GetActiveTcpConnections()
        {
            List<ConnectionInfo> connections = new();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            TcpConnectionInformation[] tcpConnections = properties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation connection in tcpConnections)
            {
                connections.Add(new ConnectionInfo
                {
                    LocalAddress = connection.LocalEndPoint.Address.ToString(),
                    LocalPort = connection.LocalEndPoint.Port,

                    RemoteAddress = connection.RemoteEndPoint.Address.ToString(),
                    RemotePort = connection.RemoteEndPoint.Port,

                    State = connection.State.ToString()
                });
            }

            return connections;
        }
    }
}