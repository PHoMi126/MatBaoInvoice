using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using SAPbouiCOM;
using System.IO;
using MatBaoInvoice.Models;

namespace MatBaoInvoice.Global
{
    public static class Globals
    {
        private static Application SBO_Application;
        private static SAPbobsCOM.Company oCompany;

        public static Application SapApplication { get { return SBO_Application; } }
        public static SAPbobsCOM.Company SapCompany { get { return oCompany; } }

        private static string path;

        private static readonly string[] Units = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
        private static readonly string[] Teens = { "mười", "mười một", "mười hai", "mười ba", "mười bốn", "mười lăm", "mười sáu", "mười bảy", "mười tám", "mười chín" };
        private static readonly string[] Tens = { "", "", "hai mươi", "ba mươi", "bốn mươi", "năm mươi", "sáu mươi", "bảy mươi", "tám mươi", "chín mươi" };
        private static readonly string[] ThousandsGroups = { "", "nghìn", "triệu", "tỷ" };

        public static string ConvertToVietnamese(decimal number)
        {
            if (number == 0)
                return "không đô la Mỹ";

            string dollarsPart = ConvertToWords((long)Math.Floor(number));
            string centsPart = ConvertToWords((long)((number - Math.Floor(number)) * 100));

            string result = $"{dollarsPart} đô la Mỹ";
            if (!string.IsNullOrEmpty(centsPart))
            {
                result += $" và {centsPart} cen";
            }

            return result.Trim();
        }

        public static string ConvertToWords(long number)
        {
            if (number == 0)
                return "";

            int thousands = 0;
            string words = "";

            while (number > 0)
            {
                int chunk = (int)(number % 1000);

                if (chunk != 0)
                {
                    string chunkText = $"{ChunkToWords(chunk)} {ThousandsGroups[thousands]}";
                    words = string.IsNullOrEmpty(words) ? chunkText : $"{chunkText} {words}";
                }

                number /= 1000;
                thousands++;
            }

            return words.Trim();
        }

        public static string ChunkToWords(int number)
        {
            string hundreds = "";
            string tens = "";

            if (number > 99)
            {
                hundreds = $"{Units[number / 100]} trăm";
                number %= 100;
            }

            if (number > 19)
            {
                tens = $"{Tens[number / 10]}";
                number %= 10;
            }
            else if (number > 9)
            {
                tens = Teens[number - 10];
                number = 0;
            }

            string units = Units[number];

            return $"{hundreds} {tens} {units}".Trim();
        }

        public static string NumberToText(double inputNum, bool suffix = true)
        {
            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
            bool isNegative = false;

            // -12345678.3445435 => "-12345678"
            string sNum = inputNum.ToString("#");
            double num = Convert.ToDouble(sNum);

            if (num < 0)
            {
                num = -num;
                sNum = num.ToString();
                isNegative = true;
            }

            int ones, tens, hundreds;
            int positionDigit = sNum.Length;
            string result = " ";

            if (positionDigit == 0)
                result = unitNumbers[0] + result;
            else
            {
                int placeValue = 0;

                while (positionDigit > 0)
                {
                    // Check last 3 digits remain ### (hundreds tens ones)
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sNum.Substring(positionDigit - 1, 1));
                    positionDigit--;
                    if (positionDigit > 0)
                    {
                        tens = Convert.ToInt32(sNum.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            hundreds = Convert.ToInt32(sNum.Substring(positionDigit - 1, 1));
                            positionDigit--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        result = placeValues[placeValue] + result;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if ((ones == 1) && (tens > 1))
                        result = "một " + result;
                    else
                    {
                        if ((ones == 5) && (tens > 0))
                            result = "lăm " + result;
                        else if (ones > 0)
                            result = unitNumbers[ones] + " " + result;
                    }
                    if (tens < 0)
                        break;
                    else
                    {
                        if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                        if (tens == 1) result = "mười " + result;
                        if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                    }
                    if (hundreds < 0) break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            result = unitNumbers[hundreds] + " trăm " + result;
                    }
                    result = " " + result;
                }
            }
            result = result.Trim();
            if (isNegative) result = "Âm " + result;
            return char.ToUpper(result[0]) + result.Substring(1) + (suffix ? " đồng chẵn" : "");
        }

        public static string ToVerbalCurrency(this double value)
        {
            var valueString = value.ToString("N2");
            var decimalString = valueString.Substring(valueString.LastIndexOf('.') + 1);
            var wholeString = valueString.Substring(0, valueString.LastIndexOf('.'));

            var valueArray = wholeString.Split(',');

            var unitsMap = new[] { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín", "mười", "mười một", "mười hai", "mười ba", "mười bốn", "mười năm", "mười sáu", "mười bảy", "mười tám", "mười chín" };
            var tensMap = new[] { "", "mười", "hai mươi", "ba mươi", "bốn mươi", "lăm mươi", "sáu mươi", "bảy mươi", "tám mươi", "chín mươi" };
            var placeMap = new[] { "", " nghìn ", " triệu ", " tỷ ", " nghìn tỷ " };

            var outList = new List<string>();

            var placeIndex = 0;

            for (int i = valueArray.Length - 1; i >= 0; i--)
            {
                var intValue = int.Parse(valueArray[i]);
                var tensValue = intValue % 100;

                var tensString = string.Empty;
                if (tensValue < unitsMap.Length) tensString = unitsMap[tensValue];
                else tensString = tensMap[(tensValue - tensValue % 10) / 10] + " " + unitsMap[tensValue % 10];

                var fullValue = string.Empty;
                if (intValue >= 100) fullValue = unitsMap[(intValue - intValue % 100) / 100] + " trăm " + tensString + placeMap[placeIndex++];
                else if (intValue != 0) fullValue = tensString + placeMap[placeIndex++];
                else placeIndex++;

                outList.Add(fullValue);
            }

            var intCentsValue = int.Parse(decimalString);

            var centsString = string.Empty;
            if (intCentsValue < unitsMap.Length) centsString = unitsMap[intCentsValue];
            else centsString = tensMap[(intCentsValue - intCentsValue % 10) / 10] + " " + unitsMap[intCentsValue % 10];

            if (intCentsValue == 0) centsString = "không";

            var output = string.Empty;
            for (int i = outList.Count - 1; i >= 0; i--) output += outList[i];
            if (centsString.ToLower().Equals("không"))
                output += " đô la";
            else
                output += " đô la và " + centsString + " cent";
            string word = output.Replace("  ", " ");
            return char.ToUpper(word[0]) + word.Substring(1);
        }

        public static void GetInfoInvoice(ref DataTable datatable)
        {
            Form oForm = SBO_Application.Forms.GetForm("0", 0);

            try
            {
                datatable = oForm.DataSources.DataTables.Add("Datatable");
            }
            catch
            {
                datatable = oForm.DataSources.DataTables.Item("Datatable");
            }

            try
            {
                datatable.ExecuteQuery("SELECT TOP 1 U_Inv_Code, U_Inv_No, U_TaxCode, U_User, U_Pass FROM [@C_Invoice]");
            }
            catch { }
        }

        public static DataTable GetSapDataTable(string sql)
        {
            DataTable oDataTable = null;

            try
            {
                string second = DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss");
                Form oForm = SBO_Application.Forms.GetForm("0", 0);

                try
                {
                    oDataTable = oForm.DataSources.DataTables.Item("TableQuery" + second);
                }
                catch
                {
                    oDataTable = oForm.DataSources.DataTables.Add("TableQuery" + second);
                }
                oDataTable.ExecuteQuery(sql);
                return oDataTable;
            }
            catch { }
            return oDataTable;
        }

        public static void GetConditions(ref Conditions oConditions, string field, string val, BoConditionOperation operation, BoConditionRelationship relationship)
        {
            Condition oCondition = default(Condition);
            oCondition = oConditions.Add();
            //oCondition.BracketOpenNum = backetNumber
            oCondition.Alias = field;
            oCondition.Operation = operation;
            oCondition.CondVal = val;
            //oCondition.BracketCloseNum = backetNumber
            oCondition.Relationship = relationship;
        }

        #region Connect to SAPB1
        public static bool SetApplication()
        {
            string sConnectionString;
            string sCookie = null;
            string sConnectionContext = null;
            SboGuiApi SboGuiApi;
            SboGuiApi = new SboGuiApi();

            if (Environment.GetCommandLineArgs().Length > 1)
                sConnectionString = Environment.GetCommandLineArgs()[1];
            else
                sConnectionString = Environment.GetCommandLineArgs()[0];

            try
            {
                SboGuiApi.Connect(sConnectionString);
                SBO_Application = SboGuiApi.GetApplication();
                oCompany = new SAPbobsCOM.Company();
                sCookie = oCompany.GetContextCookie();
                sConnectionContext = SBO_Application.Company.GetConnectionContext(sCookie);
                if (oCompany.Connected == true)
                    oCompany.Disconnect();
                if (oCompany.SetSboLoginContext(sConnectionContext) != 0)
                    return false;
                if (oCompany.Connect() != 0)
                    return false;

                SBO_Application.AppEvent += new _IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
                SetPath();

                if (Directory.Exists(path))
                    Directory.Delete(path, true);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void SBO_Application_AppEvent(BoAppEventTypes AppEventType)
        {
            switch (AppEventType)
            {
                case BoAppEventTypes.aet_ShutDown:
                    System.Windows.Forms.Application.Exit();
                    break;
                case BoAppEventTypes.aet_CompanyChanged:
                    System.Windows.Forms.Application.Exit();
                    break;
            }
        }

        private static void SetPath()
        {
            path = System.Windows.Forms.Application.StartupPath + @"\Category\" + oCompany.CompanyName;
        }
        #endregion
    }
}
