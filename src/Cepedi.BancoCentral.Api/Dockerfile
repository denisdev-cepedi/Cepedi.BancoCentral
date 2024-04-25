# Stage 1 - Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers

workdir /src
COPY ["Cepedi.BancoCentral.Api/Cepedi.BancoCentral.Api.csproj", "Cepedi.BancoCentral.Api/"]
#RUN dotnet restore "src/Cepedi.BancoCentral.Api/Cepedi.BancoCentral.Api.csproj"
#
## Copy everything else and build
#COPY . .
#WORKDIR /app/src/Cepedi.BancoCentral.Api
#RUN dotnet build -c Release -o /app/build
#
## Stage 2 - Publish the application
#FROM build AS publish
#RUN dotnet publish -c Release -o /app/publish
#
## Stage 3 - Final image
#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Cepedi.BancoCentral.Api.dll"]
