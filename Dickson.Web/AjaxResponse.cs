namespace Dickson.Web
{
    /// <summary>
    /// 描述一个Ajax请求的响应。
    /// </summary>
    public class AjaxResponse
    {
        public AjaxResponse()
        { }

        public AjaxResponse(bool succeeded, string message = null)
        {
            Succeeded = succeeded;
            Message = message;
        }

        /// <summary>
        /// 请求是否成功。
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// 人类可读的友好消息。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 创建一个不包含数据的成功响应。
        /// </summary>
        /// <param name="message">成功的消息。</param>
        /// <returns>表示成功的响应。</returns>
        public static AjaxResponse Ok(string message = null)
        {
            return new AjaxResponse(true, message);
        }
        /// <summary>
        /// 创建一个不包含数据的失败响应。
        /// </summary>
        /// <param name="message">错误的消息。</param>
        /// <returns>表示失败的响应。</returns>
        public static AjaxResponse Fail(string message = null)
        {
            return new AjaxResponse(false, message);
        }
    }

    public class AjaxResponse<T> : AjaxResponse
    {
        public AjaxResponse()
        { }

        public AjaxResponse(bool succeeded, string message, T data)
        {
            Succeeded = succeeded;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// 响应包含的数据。
        /// </summary>
        public T Data { get; set; }
    }
}
