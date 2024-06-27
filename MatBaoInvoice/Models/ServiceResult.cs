using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatBaoInvoice.Invoice;

namespace MatBaoInvoice.Models
{
    class ServiceResult
    {
        #region "Property"
        public string Message { get; set; }
        public string ResponseContent { get; set; }
        public string Status { get; set; }
        public string InvNo { get; set; } // Số hóa đơn nếu thành công

        /// <summary>
        /// Trạng thái: true-thành công / false-thất bại
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mã lỗi
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Giá trị trả về: Nội dung lỗi, dữ liệu
        /// </summary>
        public object Data
        {
            get
            {
                string str = string.Empty;

                if (m_Data != null)
                {
                    if (m_Data.GetType() == typeof(string))
                    {
                        str = (string)m_Data;
                    }
                    else
                    {
                        str = SerializeUtil.SerializeObject(m_Data);
                    }
                }

                return str;
            }
            set { m_Data = value; }
        }

        private object m_Data;

        /// <summary>
        /// Nội dung lỗi
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Nội dung cấu hình riêng nếu có
        /// </summary>
        public string CustomData { get; set; }

        #endregion "Property"

        #region "Sub/Func"

        /// <summary>
        /// Phương thức khởi tạo đổi tượng <see cref="ServiceResult"/>
        /// </summary>
        public ServiceResult()
        {
            this.Success = true;
            this.Errors = new List<string>();
            this.ErrorCode = "";
            this.CustomData = "";
        }

        #endregion "Sub/Func"
    }
}
