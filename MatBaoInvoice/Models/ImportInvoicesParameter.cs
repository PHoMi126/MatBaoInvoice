using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatBaoInvoice.Models
{
    public class ImportInvoicesParameter
    {
        public string ApiUserName { get; set; }
        public string ApiPassword { get; set; }
        public string ApiInvPattern { get; set; }
        public string ApiInvSerial { get; set; }
        public List<Invoices> Invoices { get; set; }
    }

    public class Invoices
    {
        public string FKey { get; set; }
        public string MaKH { get; set; }
        public string Buyer { get; set; } //Tên người mua - Tổ chức
        public string CusName { get; set; } //Tên người mua - khacks lẻ
        public string CusEmail { get; set; } //Email KH
        public string CusEmailCC { get; set; } //Email
        public string CusAddress { get; set; }
        public string CusPhone { get; set; }
        public string CusTaxCode { get; set; }
        public string CusBankName { get; set; }
        public string CusBankNo { get; set; }
        public string PaymentMethod { get; set; }
        public string ArisingDate { get; set; }
        public double Total { get; set; }
        public double DiscountAmount { get; set; }
        public double VATAmount { get; set; }
        public double Amount { get; set; }
        public string AmountInWords { get; set; }
        public string SO { get; set; } //Sale Order
        public int InvType { get; set; }
        public string DonViTienTe { get; set; } //704: VND, 124: Canadian $, 804: American $, ...
        public double TyGia { get; set; }
        public string CMND { get; set; }
        public string Extra { get; set; }
        public string Extra1 { get; set; }
        public string CreateBy { get; set; }
        public int Allow_signature { get; set; }
        public int InvNo { get; set; } //Số hóa đơn bị điều chỉnh
        public string InvPatternOld { get; set; } //Mẫu số của hóa đơn bị điều chỉnh
        public string InvSerialOld { get; set; } //Ký hiệu của hóa đơn
        public string Option { get; set; }
        public List<Products> Products { get; set; }
    }

    public class Products
    {
        public string Code { get; set; } //Mã SP
        public string ProdName { get; set; }
        public string ProdUnit { get; set; }
        public double ProdQuantity { get; set; }
        public double DiscountAmount { get; set; } //Giảm giá
        public double Discount { get; set; } //Giảm giá (%)
        public double ProdPrice { get; set; }
        public double VATRate { get; set; } //Thuế (%)
        public double VATAmount { get; set; } //Thuế
        public double Total { get; set; } //Tổng tiền chưa tính thuế
        public double Amount { get; set; } //Tổng tiền đã tính thuế
        public string Remark { get; set; } //Ghi chú
        public string ConNo { get; set; } //Ghi chú 1
        public string ExpDate { get; set; } //Ghi chú 2
        public string Extra { get; set; } //Ghi chú 3
        public string Extra1 { get; set; } //Ghi chú 4
        public string Extra2 { get; set; } //Ghi chú 5
        public string Extra3 { get; set; } //Ghi chú 6
        public string Extra4 { get; set; } //Ghi chú 7
        public string Extra5 { get; set; } //Ghi chú 8
        public string Extra6 { get; set; } //Ghi chú 9
        public string Extra7 { get; set; } //Ghi chú 10
        public string Extra8 { get; set; } //Ghi chú 11
        public string Extra9 { get; set; } //Ghi chú 12
        public string Extra10 { get; set; } //Ghi chú 13
        public int ProdAttr { get; set; } //1: Hàng hóa/dịch vụ, - product/service, 2: Khuyến mãi - Promotions, 3: Chiết khấu - Discount, 4: Ghi chú - Notes
    }
}
