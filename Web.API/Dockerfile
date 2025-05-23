## See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
## This stage is used when running from VS in fast mode (Default for Debug configuration)
#FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
##USER $APP_UID
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#
## This stage is used to build the service project
#FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
##COPY ["Web.API/Web.Api.csproj", "Web.API/"]
#COPY ["RelatoX.Application/RelatoX.Application.csproj", "RelatoX.Application/"]
#COPY ["RelatoX.Domain/RelatoX.Domain.csproj", "RelatoX.Domain/"]
#COPY ["RelatoX.Infra/RelatoX.Infra.csproj", "RelatoX.Infra/"]
#COPY ["RelatoX.Persistence/RelatoX.Persistence.csproj", "RelatoX.Persistence/"]
#COPY ["RelatoX.Tests/RelatoX.Tests.csproj", "RelatoX.Tests/"]
#RUN dotnet restore "./Web.API/Web.Api.csproj"
#COPY . .
#WORKDIR "/src/Web.API"
#RUN dotnet build "./Web.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## This stage is used to publish the service project to be copied to the final stage
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./Web.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
#FROM base AS final
##USER $APP_UID
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Web.Api.dll"]

# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#USER $APP_UID
WORKDIR /app

# Copia o csproj e restaura as depend�ncias
#COPY *.csproj ./
#RUN dotnet restore

COPY ["Web.API/Web.Api.csproj", "Web.API/"]
COPY ["RelatoX.Application/RelatoX.Application.csproj", "RelatoX.Application/"]
COPY ["RelatoX.Domain/RelatoX.Domain.csproj", "RelatoX.Domain/"]
COPY ["RelatoX.Infra/RelatoX.Infra.csproj", "RelatoX.Infra/"]
COPY ["RelatoX.Persistence/RelatoX.Persistence.csproj", "RelatoX.Persistence/"]
COPY ["RelatoX.Tests/RelatoX.Tests.csproj", "RelatoX.Tests/"]
RUN dotnet restore "./Web.API/Web.Api.csproj"

# Copia o restante dos arquivos e compila
COPY . .
#RUN dotnet publish -c Release -o /app/publish
RUN dotnet publish Web.API/Web.Api.csproj -c Release -o /app/publish



# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Exp�e a porta usada pela aplica��o
EXPOSE 80

# Comando de entrada
ENTRYPOINT ["dotnet", "Web.Api.dll"]
