using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatBaoInvoice.Session
{
    class Session
    {
        internal static string apiUserName = "admin";
        internal static string apiPassword = "Gtybf@12sd";
        internal static string apiInvPattern = "1";
        internal static string apiInvSerial = "C24TAT";
        internal static string FKey = "";

        internal static string API2URL = "https://api-demo.matbao.in/api/v2";

        //Khai báo đối tượng hóa đơn có mã hay không mã: có mã(true),không mã(false)
        internal static bool IsInvoiceWithCode = true;
    }
}
