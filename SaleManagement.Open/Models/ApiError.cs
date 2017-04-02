namespace SaleManagement.Open.Models
{
    public class ApiError
    {
        /// <summary>
        /// 错误代码。
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 错误的详细信息。
        /// </summary>
        public string Message { get; set; }
    }
}