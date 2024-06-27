using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatBaoInvoice.Models
{
    class OperationResults
    {
        public object Data { get; set; } //Return data
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public List<string>Errors { get; set; } //Return error
        public string CustomData { get; set; } //Custom data (if exist)
    }
}
