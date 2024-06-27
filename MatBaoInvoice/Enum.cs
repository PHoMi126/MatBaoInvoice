using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatBaoInvoice.Enums
{
    class Enum
    {
        public enum InvoiceReferenceType : int
        {
            /// <summary>
            /// Hóa đơn gốc
            /// </summary>
            Original = 0,

            /// <summary>
            /// Hóa đơn thay thế
            /// </summary>
            Replacement = 1,

            /// <summary>
            /// Hóa đơn điều chỉnh
            /// </summary>
            Adjustment = 2
        }

        /// <summary>
        /// Trạng thái thông báo sai sót
        /// </summary>
        public enum SendToTaxStatus : int
        {
            /// <summary>
            /// chưa gửi CQT
            /// </summary>
            NotSend = 0,

            /// <summary>
            /// Đã gửi CQT
            /// </summary>
            SentToTax = 1,

            /// <summary>
            /// CQT Tiếp nhận
            /// </summary>
            Received = 2,

            /// <summary>
            /// CQT không tiếp nhận
            /// </summary>
            UnReceived = 3,

            /// <summary>
            /// Gửi lỗi
            /// </summary>
            SendError = 4
        }

        /// <summary>
        /// Trạng thái phát hành hóa đơn lên MISA meInvoice
        /// </summary>
        public enum SaveInvoiceSatus : int
        {
            /// <summary>
            /// Thất bại
            /// </summary>
            Failed = 0,

            /// <summary>
            /// Thành công
            /// </summary>
            Successed = 1
        }

        public enum InvoiceTransmissionStatus : int
        {
            /// <summary>
            /// chưa gửi CQT
            /// </summary>
            NotSend = 0,

            /// <summary>
            /// Đã gửi CQT
            /// </summary>
            SentToTax = 1,

            /// <summary>
            /// CQT Tiếp nhận
            /// </summary>
            Received = 2,

            /// <summary>
            /// CQT không tiếp nhận
            /// </summary>
            UnReceived = 3,

            /// <summary>
            /// Gửi lỗi
            /// </summary>
            SendError = 4
        }

        /// <summary>
        /// Trạng thái cấp mã
        /// </summary>
        public enum GrantCodeStatus : int
        {
            /// <summary>
            /// Chờ cấp mã
            /// </summary>
            None = 0,

            /// <summary>
            /// gửi lỗi
            /// </summary>
            SendError = 1,

            /// <summary>
            /// Đã cấp mã
            /// </summary>
            Granted = 2,

            /// <summary>
            /// Từ chối cấp mã
            /// </summary>
            Rejected = 3
        }

        /// <summary>
        /// Các loại ký số
        /// </summary>
        public enum SignatureProvider
        {
            /// <summary>
            /// local
            /// </summary>
            USBToken = 0,

            /// <summary>
            /// Server
            /// </summary>
            VNPTHsm = 1,

            /// <summary>
            /// MISA eSign
            /// </summary>
            MISAESign = 2,

            /// <summary>
            /// Server
            /// </summary>
            CYBERLotusHsm = 3,

            /// <summary>
            /// ký trên máy chủ SoftDream (hsm EasyCA)
            /// </summary>
            SoftDreamHsm = 4
        }

        // <summary>
        /// Trạng thái gửi hóa đơn
        /// </summary>
        public enum SendInvoiceStatus : int
        {
            /// <summary>
            /// Chưa gửi
            /// </summary>
            NotSend = 0,

            /// <summary>
            /// Đang gửi
            /// </summary>
            Sending = 1,

            /// <summary>
            /// Gửi lỗi
            /// </summary>
            SendError = 2,

            /// <summary>
            /// Đã gửi
            /// </summary>
            Sent = 3
        }

        /// <summary>
        /// Loại tải về
        /// </summary>
        public class DownloadType
        {
            /// <summary>
            /// Tải XML
            /// </summary>
            public const string XML = "xml";

            /// <summary>
            /// Tải PDF
            /// </summary>
            public const string PDF = "pdf";

            /// <summary>
            /// Tải file zip gồm cả XML và PDF
            /// </summary>
            public const string ALL = "all";
        }
    }
}
