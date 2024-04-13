using Cepedi.BancoCentral.Compartilhado.Enums;

namespace Cepedi.BancoCentral.Compartilhado.Exceptions;
public class SemResultadosException : ApplicationException
{
    public SemResultadosException() : 
        base(BancoCentralMensagemErrors.SemResultados)
    {
    }
}
