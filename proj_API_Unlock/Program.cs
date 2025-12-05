using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;

namespace proj_API_Unlock
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            string clientId = ConfigurationManager.AppSettings["ClientId"];
            string accessToken = ConfigurationManager.AppSettings["AccessToken"];
            string apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
            bool useMock = bool.TryParse(ConfigurationManager.AppSettings["UseMock"], out var m) && m;

            Console.Write("Digite o lockId (número) que deseja abrir e pressione Enter: ");
            var line = Console.ReadLine();
            if (!int.TryParse(line, out int lockId))
            {
                Console.WriteLine("Valor inválido. O lockId deve ser um número inteiro.");
                return;
            }

            if (useMock)
            {
                // Usa o Mock
                var mock = new MockTtlockClient();
                Console.WriteLine("Modo MOCK ativado — não será feito request externo.");
                var ok = await mock.UnlockAsync(lockId);
                Console.WriteLine(ok ? "MOCK: Sucesso (fechadura simulada aberta)." : "MOCK: Falha ao abrir (simulação).");
            }
            else
            {
                // Usa o cliente real
                using (var httpClient = new HttpClient())
                {
                    var api = new TtlockClient(httpClient, clientId, accessToken, apiBaseUrl);
                    try
                    {
                        var ok = await api.UnlockAsync(lockId);
                        Console.WriteLine(ok ? "Fechadura destravada com sucesso (errcode = 0)." : "A chamada não retornou sucesso.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro da API: " + ex.Message);
                    }
                }
            }
        }
    }
}
