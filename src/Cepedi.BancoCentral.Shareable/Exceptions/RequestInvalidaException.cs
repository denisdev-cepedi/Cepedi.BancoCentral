using Cepedi.BancoCentral.Shareable.Enums;

namespace Cepedi.BancoCentral.Shareable.Exceptions;
public class RequestInvalidaException : ApplicationException
{
    public RequestInvalidaException(IDictionary<string, string[]> erros)
        : base(BancoCentralMensagemErrors.DadosInvalidos) => 
        Erros = erros.Select(e => $"{e.Key}: {string.Join(", ", e.Value)}");

    public IEnumerable<string> Erros { get; }
}
