using System.Diagnostics;

namespace Mithrandir_Sentinel.Services
{
    public class ProcessResolverService
    {
        public string GetProcessName(int pid)
        {
            try
            {
                Process process = Process.GetProcessById(pid);

                return process.ProcessName;
            }
            catch
            {
                return "Unknown";
            }
        }
    }
}
