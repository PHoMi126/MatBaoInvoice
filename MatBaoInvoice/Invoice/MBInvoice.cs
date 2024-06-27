using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using SAPbouiCOM;
using MatBaoInvoice.Global;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MatBaoInvoice.Models;
using System.Net.Http;
using System.Globalization;
using System.Diagnostics;

namespace MatBaoInvoice.Invoice
{
    class MBInvoice
    {
        private Application SBO_Application;
        private SAPbobsCOM.Company oCompany;

        public MBInvoice(Application SBO_Application, SAPbobsCOM.Company oCompany)
        {
            this.SBO_Application = SBO_Application;
            this.oCompany = oCompany;
        }

        public void OpenFormARInvoice(ItemEvent pval)
        {
            try
            {
                Form oForm = SBO_Application.Forms.Item(pval.FormUID);
                Item oItem = oForm.Items.Add("btn", BoFormItemTypes.it_BUTTON_COMBO);
                Item oItems = oForm.Items.Item("2");
                ButtonCombo oButtonCombo = null;
                oItem.Left = oItems.Left + oItems.Width + 5;
                oItem.Top = oItems.Top;
                oItem.Width = oItems.Width + 20;
                oItem.Height = oItems.Height;
                oItem.AffectsFormMode = false;
                oButtonCombo = (ButtonCombo)oItem.Specific;
                oButtonCombo.Caption = "Hóa Đơn";
                oButtonCombo.ValidValues.Add("Tạo HĐ", "Tạo HĐ");
                oButtonCombo.ValidValues.Add("Xem HĐ", "Xem HĐ");
                oButtonCombo.ValidValues.Add("Gửi email", "Gửi email");
                oButtonCombo.ValidValues.Add("Hủy HĐ", "Hủy HĐ");
                oButtonCombo.ValidValues.Add("Tải HĐ", "Tải HĐ");
                oButtonCombo.ExpandType = BoExpandType.et_DescriptionOnly;
            }
            catch { }
        }

        public async void ImportAndPublishInvoice(ItemEvent pVal)
        {
            Form oForm = SBO_Application.Forms.Item(pVal.FormUID);
            DBDataSource oinv = oForm.DataSources.DBDataSources.Item("OINV");
            DBDataSource ocrd = oForm.DataSources.DBDataSources.Item("OCRD");
            Documents invoice = null;
            ButtonCombo oBtnCombo = (ButtonCombo)oForm.Items.Item("btn").Specific;
            Documents inv = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
            BusinessPartners oBusPartner;
            oBusPartner = (BusinessPartners)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);
            oBusPartner.GetByKey(oinv.GetValue("CardCode", oinv.Offset));

            if (oBtnCombo.Selected == null)
                return;
            if (oBtnCombo.Selected.Value == "Tạo HĐ")
            {
                if (oForm.Mode != BoFormMode.fm_OK_MODE)
                    Globals.SapApplication.StatusBar.SetText
                        ("Cập nhập hoặc lưu phiếu trước khi phát hành hóa đơn", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                else
                {
                    try
                    {
                        string message = "";
                        string fkey = await GetFkey();

                        if (string.IsNullOrEmpty(fkey))
                        {
                            return;
                        }

                        PublishInvoiceParameters invoiceMD = BuildInvoice(oForm, ref message);

                        invoiceMD.Fkey = fkey;
                        invoiceMD.SO = "KC001"+"24";

                        if (invoiceMD != null)
                        {
                            string url = "https://api-demo.matbao.in/api/v2/invoice/importAndPublishInv";
                            ServiceResult serv = await CallARAPIAsync(invoiceMD, url);

                            if (string.IsNullOrEmpty(serv.Message))
                            {
                                invoice = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
                                string DocEntry = oinv.GetValue("DocEntry", 0).Trim();
                                invoice.GetByKey(int.Parse(DocEntry));
                                invoice.UserFields.Fields.Item("U_FKEY").Value = fkey;
                                invoice.Update();

                                Globals.SapApplication.StatusBar.SetText("Publish AR Sucess", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                            }
                            else
                            {
                                Globals.SapApplication.StatusBar.SetText("Failed to publish AR: " + serv.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);

                                if (!string.IsNullOrEmpty(serv.ErrorCode))
                                    Globals.SapApplication.StatusBar.SetText("Error code: " + serv.ErrorCode, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            }
                        }
                        else
                            Globals.SapApplication.StatusBar.SetText("Error creating AR: " + message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                    }
                    catch (Exception ex)
                    {
                        Globals.SapApplication.StatusBar.SetText("Unexpected error: " + ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                    }
                }
            }
            else if (oBtnCombo.Selected.Value == "Xem HĐ")
            {
                try
                {

                }
                catch { }
            }
            else if (oBtnCombo.Selected.Value == "Gửi email")
            {

            }
            else if (oBtnCombo.Selected.Value == "Hủy HĐ")
            {
                try
                {
                    if (oinv.GetValue("CANCELED", oinv.Offset) == "C")
                    {
                        if (oForm.Title.Contains("Cancellation"))
                        {
                            string fkey = oinv.GetValue("U_FKEY", oinv.Offset);

                            if (!string.IsNullOrWhiteSpace(fkey))
                            {
                                string message = "";
                                PublishInvoiceParameters cancelInvoice = CancelInvoice(oForm, ref message);

                                if (cancelInvoice != null)
                                {
                                    string url = "https://api-demo.matbao.in/api/v2/invoice/CancelInvoice";
                                    ServiceResult serv = await CallARAPIAsync(cancelInvoice, url);

                                    if (serv.Status == "OK")
                                        Globals.SapApplication.StatusBar.SetText("Hóa đơn hủy thành công", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                                    else
                                        Globals.SapApplication.StatusBar.SetText("Hủy hóa đơn thất bại", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                                }
                                else
                                    Globals.SapApplication.StatusBar.SetText("Lỗi: " + message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            }
                            else
                                Globals.SapApplication.StatusBar.SetText("Hóa đơn không thể hủy do chưa phát hành", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                        }
                    }
                    else if (oinv.GetValue("CANCELED", oinv.Offset) == "Y")
                    {
                        string fkey = oinv.GetValue("U_FKEY", oinv.Offset);

                        if (!string.IsNullOrEmpty(fkey))
                            Globals.SapApplication.StatusBar.SetText("Hóa đơn đã phát hành", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                        else
                            Globals.SapApplication.StatusBar.SetText("Chứng từ đã hủy", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
                    }
                }
                catch(Exception ex)
                {
                    Globals.SapApplication.StatusBar.SetText("Error: " + ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }
            else if (oBtnCombo.Selected.Value == "Tải HĐ")
            {
                try
                {
                    string fkey = oinv.GetValue("U_FKEY", oinv.Offset);
                    if (!string.IsNullOrEmpty(fkey))
                    {
                        string message = "";
                        DownloadInvoice downloadInvoice = DownInv(oForm, ref message);

                        if (downloadInvoice != null)
                        {
                            //string url = "https://api-demo.matbao.in/api/v2/invoice/DownloadPdf";
                            //ServiceResult serv = await CallARAPIAsync(downloadInvoice, url);
                            var client = new HttpClient();
                            var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, "https://api-demo.matbao.in/api/v2/invoice/DownloadPdf");
                            string jsonString = JsonConvert.SerializeObject(downloadInvoice);
                            var content = new StringContent(jsonString, null, "application/json");

                            request.Content = content;
                            var response = await client.SendAsync(request);
                            response.EnsureSuccessStatusCode();

                            var res = await response.Content.ReadAsStringAsync();
                            var resJson = JObject.Parse(res);

                            if (resJson["status"]?.ToString() == "OK")
                            {
                                string resUrl = resJson["link_file"]?.ToString();
                                Process.Start(resUrl);
                            }
                            else
                            {
                                Globals.SapApplication.StatusBar.SetText("Error: " + res, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                            }
                        }
                        else
                            Globals.SapApplication.StatusBar.SetText("Tải hóa đơn thất bại", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                    }
                }
                catch (Exception ex)
                {
                    Globals.SapApplication.StatusBar.SetText("Error: " + ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }
        }

        public PublishInvoiceParameters BuildInvoice(Form oForm, ref string messeage)
        {
            decimal exchangeRate = 1, quantity = 1, unitPrice = 0, vatDiscnt = 0;
            decimal? discountRate = 0;
            int HousActKey = -1;
            string VatGroup = "";

            List<PublishInvoiceProducts> products = new List<PublishInvoiceProducts>();

            try
            {
                HouseBankAccounts oHouseBankAccounts;
                oHouseBankAccounts = (HouseBankAccounts)oCompany.GetBusinessObject(BoObjectTypes.oHouseBankAccounts);
                DBDataSource oinv = oForm.DataSources.DBDataSources.Item("OINV");
                DBDataSource inv1 = oForm.DataSources.DBDataSources.Item("INV1");
                DBDataSource ocrd = oForm.DataSources.DBDataSources.Item("OCRD");
                int.TryParse(ocrd.GetValue("HousActKey", ocrd.Offset), out HousActKey);
                oHouseBankAccounts.GetByKey(HousActKey);

                if (oinv.GetValue("DocCur", oinv.Offset) == "VND")
                {
                    for (int i = 0; i < inv1.Size; i++)
                    {
                        unitPrice = decimal.Parse(inv1.GetValue("PriceBefDi", i).Trim());
                        quantity = decimal.Parse(inv1.GetValue("Quantity", i).Trim());
                        exchangeRate = decimal.Parse(inv1.GetValue("Rate", i).Trim());
                        discountRate = decimal.Parse(inv1.GetValue("DiscPrcnt", i).Trim());
                        vatDiscnt = decimal.Parse(inv1.GetValue("VatPrcnt", i).Trim());
                        VatGroup = inv1.GetValue("VatGroup", i).Trim();
                        string code = inv1.GetValue("Itemcode", i).Trim();
                        string prodName = inv1.GetValue("Dscription", i).Trim();
                        string prodUnit = inv1.GetValue("UomCode", i).Trim();
                        string remark = inv1.GetValue("U_Ghichu", i).Trim();

                        switch (VatGroup)
                        {
                            case "SO8":
                                VatGroup = "8";
                                break;
                            case "SO10":
                                VatGroup = "10";
                                break;
                            case "SO5":
                                VatGroup = "5";
                                break;
                            case "SO0":
                                VatGroup = "0";
                                break;
                            case "SO":
                                VatGroup = "KCT";
                                break;
                            default:
                                // Handle other cases if necessary
                                break;
                        }

                        products.Add(new PublishInvoiceProducts()
                        {
                            Code = code,
                            ProdName = prodName,
                            ProdUnit = prodUnit,
                            ProdQuantity = (double)quantity,
                            DiscountAmount = (double)(unitPrice * quantity * exchangeRate * (discountRate / 100 ?? 0)),
                            Discount = (double)discountRate,
                            ProdPrice = (double)unitPrice,
                            VATRate = (double)vatDiscnt,
                            VATAmount = (double)(unitPrice * quantity * (vatDiscnt / 100)),
                            Total = (double)(unitPrice * quantity),
                            Amount = (double)(unitPrice * quantity) + (double)(unitPrice * quantity * (vatDiscnt / 100)), // Tổng tiền hàng
                            Remark = "hhh",
                            ProdAttr = 1
                        }); 
                    }

                    string parseDate = oinv.GetValue("DocDate", 0).ToString();

                    //Parsing a date string in a specific format using InvariantCulture
                    string arisingDate = DateTime.ParseExact(parseDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                    decimal docTotal = decimal.Parse(oinv.GetValue("DocTotal", 0).Trim());
                    decimal vatAmount = decimal.Parse(oinv.GetValue("VatSum", 0).Trim());
                    double Total = (double)(docTotal - vatAmount);

                    PublishInvoiceParameters newInvoice = new PublishInvoiceParameters()
                    {
                        ApiUserName = Session.Session.apiUserName,
                        ApiPassword = Session.Session.apiPassword,
                        ApiInvPattern = Session.Session.apiInvPattern,
                        ApiInvSerial = Session.Session.apiInvSerial,
                        Fkey = "4FEB02F3B6",
                        MaKH = oinv.GetValue("CardCode", 0).Trim(),
                        Buyer = ocrd.GetValue("CardName", 0).Trim(),
                        CusName = ocrd.GetValue("CardName", 0).Trim(),
                        CusEmail = ocrd.GetValue("E_mail", 0).Trim(),
                        CusAddress = ocrd.GetValue("CardName", 0).Trim(),
                        CusTaxCode = ocrd.GetValue("LicTradNum", 0).Trim(),
                        PaymentMethod = oinv.GetValue("PeyMethod", 0).Trim(),
                        ArisingDate = arisingDate,
                        SO = oinv.GetValue("DocNum", 0).Trim(),
                        DonViTienTe = oinv.GetValue("DocCur", 0).Trim(),
                        TyGia = (double)exchangeRate,
                        VATAmount = (double)vatAmount,
                        Amount = (double)docTotal,
                        Total = Total,
                        Products = products
                    };
                    string amountInWords = Globals.ConvertToVietnamese((decimal)newInvoice.Amount);
                    newInvoice.AmountInWords = amountInWords;

                    return newInvoice;
                }
                else
                {
                    for (int i = 0; i < inv1.Size; i++)
                    {
                        unitPrice = decimal.Parse(inv1.GetValue("PriceBefDi", i).Trim());
                        quantity = decimal.Parse(inv1.GetValue("Quantity", i).Trim());
                        exchangeRate = decimal.Parse(inv1.GetValue("Rate", i).Trim());
                        discountRate = decimal.Parse(inv1.GetValue("DiscPrcnt", i).Trim());
                        vatDiscnt = decimal.Parse(inv1.GetValue("VatPrcnt", i).Trim());
                        VatGroup = inv1.GetValue("VatGroup", i).Trim();
                        string code = inv1.GetValue("Itemcode", i).Trim();
                        string prodName = inv1.GetValue("Dscription", i).Trim();
                        string prodUnit = inv1.GetValue("UomCode", i).Trim();
                        string remark = inv1.GetValue("U_Ghichu", i).Trim();

                        switch (VatGroup)
                        {
                            case "SO8":
                                VatGroup = "8";
                                break;
                            case "SO10":
                                VatGroup = "10";
                                break;
                            case "SO5":
                                VatGroup = "5";
                                break;
                            case "SO0":
                                VatGroup = "0";
                                break;
                            case "SO":
                                VatGroup = "KCT";
                                break;
                            default:
                                // Handle other cases if necessary
                                break;
                        }

                        products.Add(new PublishInvoiceProducts()
                        {
                            Code = code,
                            ProdName = prodName,
                            ProdUnit = prodUnit,
                            ProdQuantity = (double)quantity,
                            DiscountAmount = (double)(unitPrice * quantity * exchangeRate * (discountRate / 100 ?? 0)),
                            Discount = (double)discountRate,
                            ProdPrice = (double)unitPrice,
                            VATRate = (double)vatDiscnt,
                            VATAmount = (double)(unitPrice * quantity * (vatDiscnt / 100)),
                            Total = (double)(unitPrice * quantity),
                            Amount = (double)(unitPrice * quantity) + (double)(unitPrice * quantity * (vatDiscnt / 100)), // Tổng tiền hàng
                            Remark = "hhh",
                            ProdAttr = 1
                        });
                    }

                    string parseDate = oinv.GetValue("DocDate", 0).ToString();

                    //Parsing a date string in a specific format using InvariantCulture
                    string arisingDate = DateTime.ParseExact(parseDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                    decimal docTotal = decimal.Parse(oinv.GetValue("DocTotal", 0).Trim());
                    decimal vatAmount = decimal.Parse(oinv.GetValue("VatSum", 0).Trim());
                    double Total = (double)(docTotal - vatAmount);

                    PublishInvoiceParameters newInvoice = new PublishInvoiceParameters()
                    {
                        ApiUserName = Session.Session.apiUserName,
                        ApiPassword = Session.Session.apiPassword,
                        ApiInvPattern = Session.Session.apiInvPattern,
                        ApiInvSerial = Session.Session.apiInvSerial,
                        Fkey = "4FEB02F3B6",
                        MaKH = oinv.GetValue("CardCode", 0).Trim(),
                        Buyer = ocrd.GetValue("CardName", 0).Trim(),
                        CusName = ocrd.GetValue("CardName", 0).Trim(),
                        CusEmail = ocrd.GetValue("E_mail", 0).Trim(),
                        CusAddress = ocrd.GetValue("CardName", 0).Trim(),
                        CusTaxCode = ocrd.GetValue("LicTradNum", 0).Trim(),
                        PaymentMethod = oinv.GetValue("PeyMethod", 0).Trim(),
                        ArisingDate = arisingDate,
                        SO = oinv.GetValue("DocNum", 0).Trim(),
                        DonViTienTe = oinv.GetValue("DocCur", 0).Trim(),
                        TyGia = (double)exchangeRate,
                        VATAmount = (double)vatAmount,
                        Amount = (double)docTotal,
                        Total = Total,
                        Products = products
                    };
                    string amountInWords = Globals.ConvertToVietnamese((decimal)newInvoice.Amount);
                    newInvoice.AmountInWords = amountInWords;

                    return newInvoice;
                }
            }
            catch(Exception ex)
            {
                messeage = ex.Message;
                return null;
            }
        }

        public async Task<ServiceResult> CallARAPIAsync(PublishInvoiceParameters p, string url)
        {
            string json = JsonConvert.SerializeObject(p);

            try
            {
                using (var client = new HttpClient())
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, url);
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    var res = await response.Content.ReadAsStringAsync();

                    var responseObject = JObject.Parse(res);
                    var status = responseObject["status"].ToString();
                    var message = responseObject["messages"]?.ToString();
                    var data = responseObject["data"] as JArray;

                    var serviceResult = new ServiceResult
                    {
                        Status = status,
                        Message = message,
                        ResponseContent = res
                    };

                    if (status == "OK")
                    {
                        serviceResult.Success = true;
                        serviceResult.InvNo = data?[0]?["InvNo"]?.ToString();
                    }
                    else
                    {
                        serviceResult.Success = false;
                        if (message.Contains("Login Success"))
                        {
                            serviceResult.ErrorCode = "ERR:1";
                        }
                        else if (message.Contains("Login Fail"))
                        {
                            serviceResult.ErrorCode = "ERR:2";
                        }
                    }

                    return serviceResult;
                }
            }
            catch (HttpRequestException ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Request error: " + ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Unexpected error: " + ex.Message
                };
            }
        }

        public PublishInvoiceParameters CancelInvoice(Form oForm, ref string message)
        {
            DBDataSource oinv = oForm.DataSources.DBDataSources.Item("OINV");

            var cancel = new PublishInvoiceParameters
            {
                ApiUserName = Session.Session.apiUserName,
                ApiPassword = Session.Session.apiPassword,
                ApiInvPattern = Session.Session.apiInvPattern,
                ApiInvSerial = Session.Session.apiInvSerial,
                Fkey = oinv.GetValue("U_FKEY", 0).Trim()
            };
            return cancel;
        }

        public DownloadInvoice DownInv(Form oForm, ref string message)
        {
            DBDataSource oinv = oForm.DataSources.DBDataSources.Item("OINV");

            var download = new DownloadInvoice
            {
                ApiUserName = Session.Session.apiUserName,
                ApiPassword = Session.Session.apiPassword,
                ApiInvPattern = Session.Session.apiInvPattern,
                ApiInvSerial = Session.Session.apiInvSerial,
                Signture_type = 1,
                Fkey = oinv.GetValue("U_FKEY", 0).Trim()
            };

            return download;
        }

        public async Task<string> GetFkey()
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, "https://api-demo.matbao.in/api/v2/invoice/GetFkey");
                var content = new StringContent("{\r\n    " +
                    "\"ApiUserName\": \"admin\",\r\n    " +
                    "\"ApiPassword\": \"Gtybf@12sd\",\r\n    " +
                    "\"ApiInvPattern\":\"1\",\r\n    " +
                    "\"ApiInvSerial\":\"C24TAT\"\r\n}", null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                {
                    return "";
                }

                var res = await response.Content.ReadAsStringAsync();

                var resJson = JObject.Parse(res);
                if (resJson["status"]?.ToString() == "OK")
                {
                    return resJson["fkey"]?.ToString();
                }
                return "";
            }
        }

        public void MenuEvent(MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        public void FORM_LOAD(string formUID, ItemEvent pVal, bool BubbleEvent)
        {
            if (pVal.ActionSuccess == true && pVal.FormTypeEx == "133" || pVal.FormTypeEx == "60091") //AR Invoice load event
                OpenFormARInvoice(pVal);
        }

        public void ITEM_PRESSED(string formUID, ItemEvent pVal, bool BubbleEvent)
        {
            if (pVal.ActionSuccess == true && pVal.FormTypeEx == "133" && pVal.ItemUID == "btn")
                ImportAndPublishInvoice(pVal);
        }

        public void FORM_DATA_ADD(string FormUID, BusinessObjectInfo events, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        public void FORM_DATA_UPDATE(string FormUID, BusinessObjectInfo events, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
    }
}
