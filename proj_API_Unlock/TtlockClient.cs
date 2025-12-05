using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.Script.Serialization; // funciona no .NET Framework

namespace proj_API_Unlock
{
    // Cliente de comunicação com a API TTLock
    public class TtlockClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _accessToken;
        private readonly string _apiBaseUrl;
        private readonly JavaScriptSerializer _serializer;

        // Construtor ajustado para receber também a base da API
        public TtlockClient(HttpClient httpClient, string clientId, string accessToken, string apiBaseUrl)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            _apiBaseUrl = (apiBaseUrl ?? throw new ArgumentNullException(nameof(apiBaseUrl))).TrimEnd('/');

            // JavaScriptSerializer funciona nativamente no .NET Framework
            _serializer = new JavaScriptSerializer();
        }

        // Método de destravamento da fechadura TTLock
        public async Task<bool> UnlockAsync(int lockId)
        {
            // Monta a URL: base + endpoint
            var url = $"{_apiBaseUrl}/v3/lock/unlock";

            // Timestamp em milissegundos desde 1970 (padrão TTLock)
            long date = (long)(DateTime.UtcNow
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)))
                .TotalMilliseconds;

            // Monta o corpo como application/x-www-form-urlencoded
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("clientId", _clientId),
                new KeyValuePair<string, string>("accessToken", _accessToken),
                new KeyValuePair<string, string>("lockId", lockId.ToString()),
                new KeyValuePair<string, string>("date", date.ToString())
            });

            // Envia POST para TTLock
            using (var response = await _httpClient.PostAsync(url, formContent))
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                // Se o HTTP não for 200
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"HTTP error {response.StatusCode}: {responseBody}");

                // Tenta interpretar JSON
                TtlockResponse ttResp;
                try
                {
                    ttResp = _serializer.Deserialize<TtlockResponse>(responseBody);
                }
                catch (Exception ex)
                {
                    throw new Exception("Falha ao interpretar o JSON de resposta da TTLock.", ex);
                }

                if (ttResp != null)
                {
                    if (ttResp.errcode != 0)
                        throw new Exception($"TTLock error {ttResp.errcode}: {ttResp.errmsg}");

                    return true;
                }

                throw new Exception("Resposta da TTLock veio vazia ou em formato inesperado.");
            }
        }
    }
}
