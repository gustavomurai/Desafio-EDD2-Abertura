Projeto Abertura Física de Dispositivo (TTLock)
**Aluno:** Gustavo Serqueira Mouraígo Serqueira Mouraí  

---

## 📌 Sobre o Projeto

Este projeto implementa a comunicação com a API TTLock para realizar a **abertura física de um dispositivo remoto**, conforme solicitado no desafio da disciplina.

O trabalho se baseia no repositório disponibilizado pelo professor e nas orientações apresentadas em aula. A solução final foi organizada, documentada e ampliada para permitir testes reais e testes simulados.


Parâmetros enviados:
- `clientId`
- `accessToken`
- `lockId`
- `date` (timestamp em milissegundos)

Quando a API retorna `errcode = 0`, significa que a fechadura foi destravada com sucesso.

---

### ✔ Modo Mock (simulação)
Para permitir que o projeto seja executado **sem credenciais reais ou sem possuir uma fechadura TTLock**, foi implementado um cliente falso (`MockTtlockClient`), ativado via configuração.

Ele simula o comportamento da API:
- Retorna sucesso instantâneo
- Permite demonstração funcional da solução
- Não precisa de internet
- Ideal para testes acadêmicos

---

## ⚙️ Configuração (App.config)

O sistema utiliza o arquivo `App.config` para definir suas variáveis de execução:

```xml
<appSettings>
  <add key="ClientId" value="MEU_CLIENT_ID" />
  <add key="AccessToken" value="MEU_ACCESS_TOKEN" />
  <add key="ApiBaseUrl" value="https://euapi.ttlock.com" />
  <add key="UseMock" value="true" />
</appSettings>
