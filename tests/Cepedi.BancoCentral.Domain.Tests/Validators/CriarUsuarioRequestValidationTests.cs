using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cepedi.BancoCentral.Compartilhado.Requests;
using Cepedi.Shareable.Requests;
using FluentAssertions;

namespace Cepedi.BancoCentral.Dominio.Tests.Validators;
public class CriarUsuarioRequestValidationTests
{
    private readonly CriarUsuarioRequestValidation _sut;
    public CriarUsuarioRequestValidationTests()
    {
        _sut = new CriarUsuarioRequestValidation();
    }

    [Fact]
    public void Dado_UmObjetoValido_QuandoCriarUsuario_RetornaSucesso()
    {
        // Act & Assert
        _sut.Validate(new CriarUsuarioRequest { Nome = "teste", 
            Cpf = "123456789102" }).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Dado_UmObjetoValido_QuandoCriarUsuario_RetornaErro()
    {
        // Act & Assert
        _sut.Validate(new CriarUsuarioRequest
        {
            Nome = "teste",
            Cpf = "112"
        }).IsValid.Should().BeFalse();
    }
}
