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
        }

        public class UI
        {
            public const string TitleSuffix = "-18K珠宝管理系统";
            public const int DefaultUserCacheExpiringMinutes = 20;
            public static readonly string DateStringFormat = "yyyy-MM-dd";
        }

        public class Misc
        {
            public static readonly string JsonResponseContentType = "text/html";
            public static readonly string OrderPrefix = "KS";
            public static readonly string ShipmentOrderPrefix = "CKD";
        }

        public class SerialNames
        {
            public static readonly string Order = "order";
            public static readonly string ShipmentOrder = "shipmentOrder";
        }

        public class SystemRole
        {
            public static readonly string CommonUser = "commonUser";
            public static readonly string Admin = "admin";
            public static readonly string CustomerService = "customerService";
            public static readonly string OutputWax = "outputWax";
            public static readonly string Module = "module";
            public static readonly string SetStone = "setStone";
            public static readonly string Design = "design";
            public static readonly string Pack = "pack";
            public static readonly string Finance = "finance";
            public static readonly string SendAndReceive = "sendAndReceive";
            public static readonly string AssistantStone = "assistantStone";
        }
    }
}
