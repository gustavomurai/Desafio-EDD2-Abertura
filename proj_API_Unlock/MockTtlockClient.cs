using System;
using System.Threading.Tasks;

namespace proj_API_Unlock
{
    // Cliente falso para testes locais: simula o unlock sem falar com a API externa.
    public class MockTtlockClient
    {
        // Simula um tempo de resposta da rede e retorna true (sucesso).
        // Você pode ajustar o comportamento para testar erros também.
        public async Task<bool> UnlockAsync(int lockId)
        {
            // Simula processamento (0.5s)
            await Task.Delay(500);

            // Para debug, mostre no console qual lockId recebeu
            Console.WriteLine($"[MOCK] Simulando UNLOCK para lockId: {lockId}");

            // Podemos simular regras: se lockId == 0 -> falha
            if (lockId <= 0) return false;

            // Simula sucesso
            return true;
        }

        // Simula geração de token por abertura (opcional)
        public async Task<string> RequestTokenPerOpenAsync()
        {
            await Task.Delay(200);
            // Retorna token fake com timestamp
            return $"mock-token-{DateTime.UtcNow:yyyyMMddHHmmss}";
        }
    }
}
