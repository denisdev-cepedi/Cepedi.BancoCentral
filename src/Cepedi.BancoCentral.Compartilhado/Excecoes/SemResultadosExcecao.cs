using Cepedi.BancoCentral.Compartilhado.Enums;

namespace Cepedi.BancoCentral.Compartilhado.Exceptions;
public class SemResultadosExcecao : ExcecaoAplicacao
{
    public SemResultadosExcecao() : 
        base(BancoCentralMensagemErrors.SemResultados)
    {
    }
}
