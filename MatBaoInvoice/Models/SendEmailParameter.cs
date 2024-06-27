using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatBaoInvoice.Models
{
    class SendEmailParameter
    {
        public string ApiUserName { get; set; }
        public string ApiPassword { get; set; }
        public string InvID { get; set; }
        public string Email { get; set; }
        public string EmailCC { get; set; }
    }
}
