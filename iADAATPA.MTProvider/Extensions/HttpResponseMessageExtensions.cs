using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iADAATPA.MTProvider.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task EnsureSuccessStatusCodeAsync<T>(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            T content = await response.Content.ReadAsAsync<T>();
            string message = string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "Response status code does not indicate success: {0} ({1}).",
                (int)response.StatusCode, response.ReasonPhrase);

            if (response.Content != null)
                response.Content.Dispose();
            throw new SimpleHttpResponseException<T>(response.StatusCode, message, content);
        }
    }

    //public class SimpleHttpResponseException : SimpleHttpResponseException<object>
    //{
    //    public SimpleHttpResponseException(HttpStatusCode statusCode, string message, object content)
    //        : base(statusCode, message, content) { }
    //}

    public class SimpleHttpResponseException<T> : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public T Content { get; private set; }

        public SimpleHttpResponseException(HttpStatusCode statusCode, string message, T content) : base(message)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}
