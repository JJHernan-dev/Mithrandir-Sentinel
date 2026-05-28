using Mithrandir_Sentinel.Models;

namespace Mithrandir_Sentinel.Services
{
    public class ThreatDetectionService
    {
        public List<SecurityAlert> AnalyzeConnections(List<ConnectionInfo> connections)
        {
            List<SecurityAlert> alerts = new();

            foreach (var connection in connections)
            {
                // ALERTA SOLO SI ES HIGH RISK

                if (connection.RiskLevel == "High")
                {
                    alerts.Add(
                        new SecurityAlert
                        {
                            Title = "High Risk Connection",

                            Description =
                                $"Suspicious connection detected from {connection.RemoteAddress}",

                            Severity = "High",

                            ProcessName = connection.ProcessName,

                            RemoteAddress = connection.RemoteAddress,

                            Timestamp = DateTime.Now,
                        }
                    );
                }

                // PUERTOS SOSPECHOSOS

                if (connection.RemotePort == 4444 || connection.RemotePort == 1337)
                {
                    alerts.Add(
                        new SecurityAlert
                        {
                            Title = "Suspicious Port Detected",

                            Description =
                                $"Connection detected on suspicious port {connection.RemotePort}",

                            Severity = "High",

                            ProcessName = connection.ProcessName,

                            RemoteAddress = connection.RemoteAddress,

                            Timestamp = DateTime.Now,
                        }
                    );
                }

                // ESTADOS INESTABLES

                if (connection.State == "CloseWait")
                {
                    alerts.Add(
                        new SecurityAlert
                        {
                            Title = "Unstable Connection State",

                            Description = $"Connection in state {connection.State}",

                            Severity = "Medium",

                            ProcessName = connection.ProcessName,

                            RemoteAddress = connection.RemoteAddress,

                            Timestamp = DateTime.Now,
                        }
                    );
                }
            }

            return alerts;
        }
    }
}
