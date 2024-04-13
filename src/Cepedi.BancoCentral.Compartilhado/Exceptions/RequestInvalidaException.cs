using Cepedi.BancoCentral.Compartilhado.Enums;

namespace Cepedi.BancoCentral.Compartilhado.Exceptions;
public class RequestInvalidaException : ApplicationException
{
    public RequestInvalidaException(IDictionary<string, string[]> erros)
        : base(BancoCentralMensagemErrors.DadosInvalidos) => 
        Erros = erros.Select(e => $"{e.Key}: {string.Join(", ", e.Value)}");

    public IEnumerable<string> Erros { get; }
}
