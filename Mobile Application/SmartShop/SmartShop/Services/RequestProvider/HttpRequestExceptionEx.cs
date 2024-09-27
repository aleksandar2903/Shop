using System;
using System.Net;
using System.Runtime.Serialization;

namespace SmartShop.Services.RequestProvider
{
    public class HttpRequestExceptionEx : Exception
    {
        public System.Net.HttpStatusCode HttpCode { get; }

        public HttpRequestExceptionEx(System.Net.HttpStatusCode code) : this(code, null, null)
        {
        }

        public HttpRequestExceptionEx(System.Net.HttpStatusCode code, string message) : this(code, message, null)
        {
        }

        public HttpRequestExceptionEx(System.Net.HttpStatusCode code, string message, Exception inner) : base(message, inner)
        {
            HttpCode = code;
        }
    }
}