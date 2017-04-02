using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SaleManagement.Open.Models
{
    public class ApiException : Exception
    {
        public ApiException(int code, string message, HttpStatusCode statusCode)
             : base(message)
        {
            if (code < 0)
                throw new ArgumentOutOfRangeException("code");

            Code = code;
            StatusCode = statusCode;
        }

        public int Code { get; private set; }

        public HttpStatusCode StatusCode { get; set; }

        public ApiError ToModel()
        {
            return new ApiError
            {
                Code = Code,
                Message = Message
            };
        }
    }
}