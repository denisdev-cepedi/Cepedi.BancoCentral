using Cepedi.BancoCentral.Shareable.Enums;

namespace Cepedi.BancoCentral.Shareable.Exceptions;
public class SemResultadosException : ApplicationException
{
    public SemResultadosException() : 
        base(BancoCentralMensagemErrors.SemResultados)
    {
    }
}
