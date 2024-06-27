using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatBaoInvoice.Models
{
    class ErrorCodes
    {
        #region Mã lỗi phiên làm việc

        /// <summary>
        /// Token hết hạn - Cần gọi hàm RefreshToken
        /// </summary>
        public const string TokenExpiredCode = "TokenExpiredCode";

        /// <summary>
        /// Token lỗi cần đăng nhập lại
        /// </summary>
        public const string InvalidTokenCode = "InvalidTokenCode";

        #endregion

        #region Validate khi tạo XML cho hóa đơn

        /// <summary>
        /// Mẫu hóa đơn không tồn tại
        /// </summary>
        public const string InvoiceTemplateNotExist = "InvoiceTemplateNotExist";

        /// <summary>
        /// Tờ khai đăng ký/thay đổi đã được Cơ quan thuế chấp nhận không tồn tại
        /// </summary>
        public const string InvoiceTemplateNotValidInDeclaration = "InvoiceTemplateNotValidInDeclaration";

        /// <summary>
        /// Nếu ngày hóa đơn không hợp lệ, nhỏ hơn ngày của hóa đơn cuối cùng đã phát hành
        /// </summary>
        public const string InvalidInvoiceDate = "InvalidInvoiceDate";

        /// <summary>
        /// Tạo XML hóa đơn lỗi không xác định
        /// </summary>
        public const string CreateInvoiceDataError = "CreateInvoiceDataError";

        /// <summary>
        /// Nếu mã lỗi bắt đầu bằng InvoiceDetail_ thì tham số sau dấu _ là trường dữ liệu không hợp lệ
        /// - Không thuộc loại được cho phép
        /// - Giá trị bắt buộc - không được trống
        /// - Vượt quá giới hạn MaxLength cho phép
        /// </summary>
        public const string InvoiceDetail_ = "InvoiceDetail_";

        /// <summary>
        /// Tên loại thuế suất trong Bảng tổng hợp thuế suất của hóa đơn có dữ liệu không hợp lệ
        /// </summary>
        public const string TaxRateInfo_ = "TaxRateInfo_VATRateName";

        #endregion Validate khi tạo XML cho hóa đơn

        #region Validate khi phát hành hóa đơn

        /// <summary>
        /// Số lượng hóa đơn gửi lên trong 1 Request quá số lượng cho phép
        /// <para>Mặc định là 50</para>
        /// </summary>
        public const string InvoiceQuantityTooLarge = "InvoiceQuantityTooLarge";

        /// <summary>
        /// Chưa mua tài nguyên
        /// </summary>
        public const string LicenseInfo_NotBuy = "LicenseInfo_NotBuy";

        /// <summary>
        /// Số lượng tài nguyên còn lại không đủ để phát hành toàn bộ các hóa đơn gửi lên
        /// </summary>
        public const string LicenseInfo_OutOfInvoice = "LicenseInfo_OutOfInvoice";

        /// <summary>
        /// Tài nguyên chưa thanh toán hoặc đã hết hạn
        /// </summary>
        public const string LicenseInfo_Expired = "LicenseInfo_Expired";

        /// <summary>
        /// Nếu bắt đầu bằng RequireError_{0} thì thông tin phía sau đang bị trống
        /// </summary>
        public const string RequireError = "RequireError";

        /// <summary>
        /// Mã tra cứu không hợp lệ
        /// </summary>
        public const string InvalidTransactionID = "InvalidTransactionID";

        /// <summary>
        /// Trùng Mã tra cứu
        /// </summary>
        public const string DuplicateTransactionID = "DuplicateTransactionID";

        /// <summary>
        /// Chữ ký số bị bỏ trống
        /// </summary>
        public const string SignatureEmpty = "SignatureEmpty";

        /// <summary>
        /// Chữ ký số không hợp lệ
        /// </summary>
        public const string InvalidSignature = "InvalidSignature";

        /// <summary>
        /// Chữ ký số đã bị thu hồi
        /// </summary>
        public const string CertRevocation = "CertRevocation";

        /// <summary>
        /// XML không hợp lệ
        /// </summary>
        public const string InvalidXMLData = "InvalidXMLData";

        /// <summary>
        /// Số hóa đơn không hợp lệ
        /// </summary>
        public const string InvalidInvNo = "InvalidInvNo";

        /// <summary>
        /// Mã số thuế không hợp lệ
        /// </summary>
        public const string InvalidTaxCode = "InvalidTaxCode";

        /// <summary>
        /// Trùng RefID của hóa đơn
        /// </summary>
        public const string DuplicateInvoiceRefID = "DuplicateInvoiceRefID";

        /// <summary>
        /// Số hóa đơn không liên tục
        /// </summary>
        public const string InvoiceNumberNotCotinuous = "InvoiceNumberNotCotinuous";

        /// <summary>
        /// Trùng hóa đơn - hóa đơn đã được phát hành
        /// </summary>
        public const string InvoiceDuplicated = "InvoiceDuplicated";

        /// <summary>
        /// Thực hiện bị Exception - Không rõ nguyên nhân
        /// </summary>
        public const string Exception = "Exception";

        /// <summary>
        /// Ngày hóa đơn giảm thuế không hợp lệ
        /// </summary>
        public const string TaxReductionDateInValid = "TaxReductionDateInValid";

        #endregion Validate khi phát hành hóa đơn

        #region Mã lỗi ký hóa đơn

        /// <summary>
        /// Không kết nối được máy chủ ký số
        /// </summary>
        public const string CannotConnectSignServer = "CannotConnectSignServer";

        /// <summary>
        /// Mã pin của chứng thư không đúng
        /// </summary>
        public const string InCorrectPinCode = "InCorrectPinCode";

        /// <summary>
        /// Chữ ký số hết hạn
        /// </summary>
        public const string ValidateExpriredate = "ValidateExpriredate";

        #endregion

        #region Mã lỗi khác

        /// <summary>
        /// Hóa đơn không tồn tại
        /// </summary>
        public const string InvoiceNotExist = "InvoiceNotExist";

        /// <summary>
        /// Chuyển đổi lỗi
        /// </summary>
        public const string ConvertError = "ConvertError";

        /// <summary>
        /// Mẫu hóa đơn không hỗ trợ
        /// </summary>
        public const string TemplateNotSupport = "TemplateNotSupport";

        #endregion
    }
}
