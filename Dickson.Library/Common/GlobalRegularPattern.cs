using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dickson.Library.Common
{
    /// <summary>
    /// 定义全局的正则表达式。
    /// </summary>
    public static class GlobalRegularPattern
    {
        /// <summary>
        /// 简单的Email表达式，适合快速宽松的场景。
        /// </summary>
        public const string SimplyEmail = @"^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$";
        /// <summary>
        /// 严格的Email表达式，适合精确的场景。
        /// </summary>
        public const string StrictEmail = @"^\s*[a-zA-Z0-9_%+#&'*/=^`{|}~-](?:\.?[a-zA-Z0-9_%+#&'*/=^`{|}~-])*@(?:[a-zA-Z0-9_](?:(?:\.?|-*)[a-zA-Z0-9_])*\.[a-zA-Z]{2,9}|\[(?:2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?:2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?:2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?:2[0-4]\d|25[0-5]|[01]?\d\d?)])\s*$";
        /// <summary>
        /// 安全的密码校验表达式，要求至少包含大小字母、小写字母、数字、特殊字符四种中的三种。
        /// </summary>
        public const string SecurityPassword = @"^(?![0-9a-z]+$)(?![0-9A-Z]+$)(?![0-9\W]+$)(?![a-z\W]+$)(?![a-zA-Z]+$)(?![A-Z\W]+$)[a-zA-Z0-9\W_]{7,20}$";
        /// <summary>
        /// 通用密码校验规则，7~20位，包含至少1个数字和字母。
        /// </summary>
        public const string SimplyPassword = "((?=.*\\d)(?=.*[a-zA-Z]).{7,20})";
        /// <summary>
        /// 简单手机号，1打头，11位数字。
        /// </summary>
        public const string SimplyMobile = @"^1[34578]\d{9}$";
        /// <summary>
        /// 简单电话。
        /// </summary>
        public const string SimplyTelephone = @"^\d[-0-9]{5,20}\d$";
    }
}
