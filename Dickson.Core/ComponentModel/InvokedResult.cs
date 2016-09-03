using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dickson.Core.ComponentModel
{
    public class InvokedResult
    {
        IReadOnlyCollection<ErrorDescriber> m_Errors;

        /// <summary>
        /// 操作是否成功。
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// 获取错误列表。
        /// </summary>
        public IReadOnlyCollection<ErrorDescriber> Errors
        {
            get
            {
                return m_Errors;
            }
            set
            {
                m_Errors = value;
            }
        }

        /// <summary>
        /// 获取错误列表的第一个错误。
        /// </summary>
        public ErrorDescriber Error
        {
            get
            {
                return m_Errors == null ? null : m_Errors.FirstOrDefault();
            }
        }

        public static readonly InvokedResult SucceededResult = new InvokedResult { Succeeded = true };

        /// <summary>
        /// 创建包含数据的成功结果。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="data">数据。</param>
        /// <returns></returns>
        public static InvokedResult<T> Ok<T>(T data)
        {
            return new InvokedResult<T> { Succeeded = true, Data = data };
        }

        public static InvokedResult Fail(string errorCode, string errorDescription)
        {
            if (errorCode == null)
                throw new ArgumentNullException("errorCode");

            var result = new InvokedResult { Succeeded = false };
            var list = new ErrorDescriber[] { new ErrorDescriber { Code = errorCode, Description = errorDescription } };
            result.Errors = list;
            return result;
        }

        public static InvokedResult Fail(ErrorDescriber error)
        {
            if (error == null)
                throw new ArgumentNullException("error");

            var result = new InvokedResult { Succeeded = false };
            var list = new ErrorDescriber[] { error };
            result.Errors = list;
            return result;
        }

        public static InvokedResult Fail(IEnumerable<ErrorDescriber> errors)
        {
            if (errors == null)
                throw new ArgumentNullException("errors");

            var result = new InvokedResult { Succeeded = false };
            var list = new List<ErrorDescriber>();
            list.AddRange(errors);
            result.Errors = list;
            return result;
        }

        public static InvokedResult<T> Fail<T>(string errorCode, string errorDescription, T data)
        {
            if (errorCode == null)
                throw new ArgumentNullException("errorCode");

            var result = new InvokedResult<T> { Succeeded = false, Data = data };
            var list = new ErrorDescriber[] { new ErrorDescriber { Code = errorCode, Description = errorDescription } };
            result.Errors = list;
            return result;
        }

        public static InvokedResult<T> Fail<T>(ErrorDescriber error, T data)
        {
            if (error == null)
                throw new ArgumentNullException("error");

            var result = new InvokedResult<T> { Succeeded = false, Data = data };
            var list = new ErrorDescriber[] { error };
            result.Errors = list;
            return result;
        }

        public static InvokedResult<T> Fail<T>(IEnumerable<ErrorDescriber> errors, T data)
        {
            if (errors == null)
                throw new ArgumentNullException("errors");

            var result = new InvokedResult<T> { Succeeded = false, Data = data };
            var list = new List<ErrorDescriber>();
            list.AddRange(errors);
            result.Errors = list;
            return result;
        }
    }

    public class InvokedResult<T> : InvokedResult
    {
        public T Data { get; set; }
    }
}
