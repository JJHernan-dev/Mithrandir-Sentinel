using System;
using System.Collections.Generic;
using System.Text;

namespace Mithrandir_Sentinel.Models
{
    public class ConnectionInfo
    {

        public string LocalAddress { get; set; } = string.Empty;

        public int LocalPort { get; set; }

        public string RemoteAddress { get; set; } = string.Empty;

        public int RemotePort { get; set; }

        public string State { get; set; } = string.Empty;

    }
}
