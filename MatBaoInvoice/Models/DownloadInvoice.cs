using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatBaoInvoice.Models
{
    public class DownloadInvoice
    {
        public string ApiUserName { get; set; }
        public string ApiPassword { get; set; }
        public string ApiInvPattern { get; set; }
        public string ApiInvSerial { get; set; }
        public int Signture_type { get; set; }
        public string Fkey { get; set; }
    }
}
