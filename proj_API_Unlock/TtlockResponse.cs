using System;

namespace proj_API_Unlock
{
    // Modelo simples para receber a resposta da API TTLock
    public class TtlockResponse
    {
        // errcode == 0 significa sucesso segundo o padrão da TTLock
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string description { get; set; }
    }
}
