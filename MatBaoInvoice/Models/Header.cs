using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatBaoInvoice.Models
{
    public class HeaderKey
    {
        /// <summary>
        /// Key header chứa chuỗi Token bảo mật
        /// </summary>
        public const string Authorization = "Authorization";

        /// <summary>
        /// Key header chứa taxcode
        /// </summary>
        public const string CompanyTaxCode = "CompanyTaxCode";
    }

    public class HttpMethod
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
    }
}
