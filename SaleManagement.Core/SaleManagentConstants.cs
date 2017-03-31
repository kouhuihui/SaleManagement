using Dickson.Core.ComponentModel;
using System.Configuration;

namespace SaleManagement.Core
{
    public class SaleManagentConstants
    {
        public class Validations
        {
            public const int PasswordStringLength = 20;
            public const int PasswordHashStringLength = 128;
            public const int EmailStringLength = 50;
            public const int UserNameStringLength = 20;
            public const int PhoneStringLength = 24;
            public const int MoblieStringLength = 11;
            public const int DefaultStringLength = 256;
            public const int DefaultNameStringLength = 50;
            public const int DefaultIdStringLength = 36;
            public const int GeneralShorterStringLength = 50;
            public const int GeneralStringLength = 128;
            public const string SimplyPassword = "((?=.*\\d)(?=.*[a-zA-Z]).{6,16})";
            public const int MaxUploadFileSize = 4 * 1024 * 1024; //4MB
        }

        public class Errors
        {
            public const string PasswordError = "请输入6位密码";
            public const string OrderNotFound = "订单不存在";
            public const string InvalidRequest = "无效请求";
        }

        public class UI
        {
            public const string TitleSuffix = "-18K珠宝管理系统";
            public const int DefaultUserCacheExpiringMinutes = 20;
            public const int DefaultExpiringDays = 7;
            public const int OrderUrgentWaringDay = 6;
            public const int OrderVeryUrgentWaringDay = 3;
            public const int OrderRushDays = 8;
            public const int OrderVeryRushDays = 4;
            public static readonly string DateStringFormat = "yyyy-MM-dd";
            public static readonly string[] MainStoneTypes = new string[] { "0.30ct", "0.40ct", "0.50-0.60ct", "0.70-0.80ct", "0.90-1.00ct" };
            public static readonly int[] HandSizes = new int[] { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
        }

        public class Misc
        {
            public static readonly string JsonResponseContentType = "text/html";
            public static readonly string OrderPrefix = "KS";
            public static readonly string ShipmentOrderPrefix = "CKD";
            public static readonly string SpotGoodsPrefix = "XH";
            public static readonly string SaleMangementWeb = ConfigurationManager.AppSettings["SaleMangementWeb"];
        }

        public class SerialNames
        {
            public static readonly string Order = "order";
            public static readonly string ShipmentOrder = "shipmentOrder";
            public static readonly string SpotGoods = "SpotGoods";
        }

        public class SystemRole
        {
            public static readonly string CommonUser = "commonUser";
            public static readonly string Admin = "admin";
            public static readonly string CustomerService = "customerService";
            public static readonly string OutputWax = "outputWax";
            public static readonly string Module = "module";
            public static readonly string Design = "design";
            public static readonly string Pack = "pack";
            public static readonly string Finance = "finance";
            public static readonly string SendAndReceive = "sendAndReceive";
            public static readonly string AssistantStone = "assistantStone";
            public static readonly string WithTheHand = "withTheHand";
            public static readonly string MicroInsert = "microInsert";
            public static readonly string Polishing = "polishing";
            public static readonly string Direktor = "direktor";
            public static readonly string OrderView = "orderView";
        }

        public class ConfigKeys
        {
            public static readonly string wxAccountCookie = "wxAccount";
        }

        public class InvokedResults
        {
            public static readonly InvokedResult UserNotFoundResult = InvokedResult.Fail("NotFound", "用户不存在");
        }
    }
}
