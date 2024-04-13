# Cepedi.CleanArch

Executar o projeto `src\Cepedi.BancoCentral.WebApi` para abrir o swagger

## Caracter�sticas do projeto

- .NET 7.0

- Projeto base para a trilha avan�ada de .net da resid�ncia TIC18. A Resid�ncia TIC18 � um projeto oferecido pela parceria entre a Uesc e o Cepedi, localizado em Ilh�us-BA.

## Aplicar migrations
 dotnet ef migrations add Init --project .\src\Cepedi.BancoCentral.Performance\ -s .\src\Cepedi.BancoCentral.WebApi\

# Remove a �ltima migration aplicada
 dotnet ef migrations remove --project .\src\Cepedi.BancoCentral.Performance\ -s .\src\Cepedi.BancoCentral.WebApi\

# Gera o script de migra��o
 dotnet ef migrations script --project .\src\Cepedi.BancoCentral.Performance\ -s .\src\Cepedi.BancoCentral.WebApi\

# Coloca o banco para o estado inicial zero
 dotnet ef migrations update 0 --project .\src\Cepedi.BancoCentral.Performance\ -s .\src\Cepedi.BancoCentral.WebApi\
