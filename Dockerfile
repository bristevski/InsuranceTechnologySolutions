
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN dotnet tool install --global dotnet-ef --version 7.0.5
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /src

COPY ["Claims/Claims.csproj", "Claims/"]
COPY ["Claims.Application/Claims.Application.csproj", "Claims.Application/"]
COPY ["Claims.Core/Claims.Core.csproj", "Claims.Core/"]
COPY ["Claims.Infrastructure/Claims.Infrastructure.csproj", "Claims.Infrastructure/"]

RUN dotnet restore "./Claims/Claims.csproj"

COPY . .

WORKDIR "/src/Claims"
RUN dotnet build "./Claims.csproj" -c Release -o /app/build
RUN dotnet publish "./Claims.csproj" -c Release -o /app/publish /p:UseAppHost=false

RUN dotnet ef database update --project /src/Claims.Infrastructure --startup-project /src/Claims --context Claims.Infrastructure.Audit.AuditContext --verbose

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Claims.dll"]
