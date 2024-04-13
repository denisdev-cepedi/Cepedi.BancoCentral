using Cepedi.Shareable.Requests;
using FluentValidation;

namespace Cepedi.BancoCentral.Compartilhado.Requests;
public class CriarUsuarioRequestValidation : 
    AbstractValidator<CriarUsuarioRequest>
{
    public CriarUsuarioRequestValidation()
    {
        RuleFor(e => e.Cpf).NotEmpty().Length(12).WithMessage("O Cpf é obrigatorio");
    }
}
