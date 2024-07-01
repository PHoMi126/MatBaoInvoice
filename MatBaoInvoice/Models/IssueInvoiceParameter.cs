using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatBaoInvoice.Models
{
    public class IssueInvoiceParameter
    {
        public string ApiUserName { get; set; }
        public string ApiPassword { get; set; }
        public string ApiInvPattern { get; set; }
        public string ApiInvSerial { get; set; }
        public string Fkey { get; set; }
        public string ArisingDate { get; set; }
    }
}
