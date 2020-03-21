using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Acme.MessageSender.Infrastructure.ApiAgents
{
	public class BaseApiAgent
	{
		private readonly ILogger _logger;
		private readonly string _baseUrl;
		private static HttpClient _httpClient;

		public static HttpClient HttpClient
		{
			get
			{
				if (_httpClient == null)
				{
					_httpClient = new HttpClient();
				}
				return _httpClient;
			}
		}

		public BaseApiAgent(string apiBaseUrl, ILogger logger)
		{
			_logger = logger;
			_baseUrl = apiBaseUrl;
		}

		#region Protected Methods

		protected async Task<BasicHttpResponse> GetAsync(string apiPath, params (string key, string value)[] queryParameters)
		{
			string uri = HttpHelper.BuildUri(_baseUrl, apiPath);
			uri = HttpHelper.AppendQuerystring(uri, queryParameters);

			_logger.LogDebug($"Execute GET request to {uri}");
			using (var httpResponse = await HttpClient.GetAsync(uri))
			{
				HandleFailedResponse(apiPath, httpResponse);

				return new BasicHttpResponse
				{
					ResponseStatusCode = (int)httpResponse.StatusCode,
					ResponseStatusReason = httpResponse.ReasonPhrase,
					Content = await httpResponse.Content.ReadAsStringAsync()
				};
			}
		}

		protected async Task<BasicHttpResponse> PostAsync(string apiPath, string jsonContent)
		{
			using (HttpContent httpContent = HttpHelper.CreateHttpJsonContent(jsonContent, null))
			{
				string uri = HttpHelper.BuildUri(_baseUrl, apiPath);
				_logger.LogDebug($"Execute POST request to {uri}");
				using (var httpResponse = await HttpClient.PostAsync(uri, httpContent))
				{
					HandleFailedResponse(apiPath, httpResponse);

					return new BasicHttpResponse
					{
						ResponseStatusCode = (int)httpResponse.StatusCode,
						ResponseStatusReason = httpResponse.ReasonPhrase,
						Content = await httpResponse.Content.ReadAsStringAsync()
					};
				}
			}
		}

		#endregion

		#region Member / Helper Methods

		private void HandleFailedResponse(string apiName, HttpResponseMessage httpResponse)
		{
			if (httpResponse.IsSuccessStatusCode) return;

			const string logLine = "Failure response while calling api at: {0}. statusCode: {1}, reasonPhrase: {2}";
			_logger.LogError(string.Format(logLine, apiName, (int)httpResponse.StatusCode, httpResponse.ReasonPhrase));
			httpResponse.EnsureSuccessStatusCode();
		}

		#endregion
	}
}
