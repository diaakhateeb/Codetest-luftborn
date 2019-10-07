using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Luftborn.Helpers
{
    /// <summary>
    /// Provides HttpClient object to call RESTfull API.
    /// </summary>
    public class ClientProvider : IDisposable
    {
        private readonly HttpClient _httpClient;
        /// <summary>
        /// Initializes ClientProvider instance. 
        /// </summary>
        public ClientProvider()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// HttpClient object property.
        /// </summary>
        public HttpClient Client => _httpClient;

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}
