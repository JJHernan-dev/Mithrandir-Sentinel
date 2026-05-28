using Mithrandir_Sentinel.Models;

namespace Mithrandir_Sentinel.Services
{
    public class RiskAssessmentService
    {
        public string CalculateRisk(ConnectionInfo connection)
        {
            if (connection.ProcessName.ToLower().Contains("powershell"))
            {
                return "High";
            }

            if (connection.RemotePort == 4444 || connection.RemotePort == 1337)
            {
                return "High";
            }

            if (connection.State == "CLOSE_WAIT")
            {
                return "Medium";
            }

            if (connection.RemotePort > 50000)
            {
                return "Medium";
            }

            return "Low";
        }
    }
}
