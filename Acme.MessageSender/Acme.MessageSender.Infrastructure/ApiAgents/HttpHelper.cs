using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Acme.MessageSender.Infrastructure.ApiAgents
{
	public class HttpHelper
	{
        public static HttpContent CreateHttpJsonContent(string content, Dictionary<string, string> headers,
            JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (content == null)
            {
                return null;
            }

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
            ms.Seek(0, SeekOrigin.Begin);
            HttpContent httpContent = new StreamContent(ms);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpContent.Headers.Add(header.Key, header.Value);
                }
            }
            return httpContent;
        }

        /// <summary>
        /// Safely builds up a URI based on a number of string parts.
        /// Ensures that each two adjacent parts of the URL is connected with no more or less than 1 "/" character.
        /// </summary>
        public static string BuildUri(params string[] uriParts)
        {
            if (uriParts.Length == 0)
            {
                return string.Empty;
            }
            else if (uriParts.Length == 1)
            {
                return uriParts[0];
            }

            StringBuilder sb = new StringBuilder(uriParts[0]);
            for (int i = 1; i < uriParts.Length; i++)
            {
                if (uriParts[i - 1].EndsWith("/") && uriParts[i].StartsWith("/"))
                {
                    uriParts[i] = uriParts[i].Remove(0, 1);
                }
                else if (!uriParts[i - 1].EndsWith("/") && !uriParts[i].StartsWith("/"))
                {
                    uriParts[i] = uriParts[i].Insert(0, "/");
                }
                sb.Append(uriParts[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Safely appends a list of key/value pairs to the given URI as a querystring and returns the result
        /// </summary>
        public static string AppendQuerystring(string uri, params (string key, string value)[] queryParameters)
        {
            if (queryParameters == null || queryParameters.Length == 0) { return uri; }

            StringBuilder sb = new StringBuilder($"{uri}?");
            for (int i = 0; i < queryParameters.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append("&");
                }

                sb.Append($"{queryParameters[i].key }={queryParameters[i].value}");
            }

            return sb.ToString();
        }
    }
}
