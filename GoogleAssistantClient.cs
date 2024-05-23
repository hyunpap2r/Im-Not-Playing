using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Im_Not_Playing
{
    internal class GoogleAssistantClient
    {
        private readonly HttpClient _client;
        private readonly string _accessToken;
        private readonly string _apiEndpoint;

        public GoogleAssistantClient(string accessToken, string apiEndpoint)
        {
            _client = new HttpClient();
            _accessToken = accessToken;
            _apiEndpoint = apiEndpoint;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }


        public async Task<string> SendQueryAsync(string query)
        {
            var requestBody = new { query = query };
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_apiEndpoint, content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }


    }
}
