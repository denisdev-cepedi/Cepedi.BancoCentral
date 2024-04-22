using Cepedi.BancoCentral.Compartilhado.Excecoes;

namespace Cepedi.BancoCentral.Compartilhado.Exceptions;
public class ExcecaoAplicacao : Exception
{
    public ExcecaoAplicacao(ResultadoErro erro)
     : base(erro.Descricao) => ResponseErro = erro;

    public ResultadoErro ResponseErro { get; set; }
}
