using System.Diagnostics;
using System.Text.RegularExpressions;
using Mithrandir_Sentinel.Models;

namespace Mithrandir_Sentinel.Services
{
    public class NetworkService
    {
        public List<ConnectionInfo> GetActiveTcpConnections()
        {
            List<ConnectionInfo> connections = new();

            ProcessStartInfo startInfo = new()
            {
                FileName = "netstat",
                Arguments = "-ano",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using Process process = Process.Start(startInfo)!;

            string output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            string[] lines = output.Split('\n');

            foreach (string line in lines)
            {
                if (!line.Trim().StartsWith("TCP"))
                    continue;

                string cleanedLine = Regex.Replace(line, @"\s+", " ").Trim();

                string[] parts = cleanedLine.Split(' ');

                if (parts.Length < 5)
                    continue;

                try
                {
                    string localEndpoint = parts[1];
                    string remoteEndpoint = parts[2];
                    string state = parts[3];
                    int pid = int.Parse(parts[4]);

                    string[] localParts = localEndpoint.Split(':');
                    string[] remoteParts = remoteEndpoint.Split(':');

                    string localAddress = string.Join(":", localParts.Take(localParts.Length - 1));

                    int localPort = int.Parse(localParts.Last());

                    string remoteAddress = string.Join(
                        ":",
                        remoteParts.Take(remoteParts.Length - 1)
                    );

                    int remotePort = int.Parse(remoteParts.Last());

                    ConnectionInfo connection = new()
                    {
                        LocalAddress = localAddress,
                        LocalPort = localPort,

                        RemoteAddress = remoteAddress,
                        RemotePort = remotePort,

                        State = state,

                        ProcessId = pid,

                        ProcessName = _processResolverService.GetProcessName(pid),
                    };

                    connection.RiskLevel = _riskAssessmentService.CalculateRisk(connection);

                    connections.Add(connection);
                }
                catch
                {
                    continue;
                }
            }

            return connections;
        }

        private readonly ProcessResolverService _processResolverService;
        private readonly RiskAssessmentService _riskAssessmentService;

        public NetworkService()
        {
            _processResolverService = new ProcessResolverService();
            _riskAssessmentService = new RiskAssessmentService();
        }
    }
}
