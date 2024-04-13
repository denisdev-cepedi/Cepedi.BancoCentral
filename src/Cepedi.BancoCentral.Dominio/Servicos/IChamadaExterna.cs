using Refit;

namespace Cepedi.BancoCentral.Dominio.Servicos;
public interface IChamadaExterna
{
    [Post("api/v1/Enviar")]
    Task<ApiResponse<HttpResponseMessage>> EnviarNotificacao([Body] object notificacao);
}
