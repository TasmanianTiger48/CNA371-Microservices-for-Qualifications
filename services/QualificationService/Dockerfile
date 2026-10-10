FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["QualificationService.csproj", "./"]
RUN dotnet restore "QualificationService.csproj"

COPY . .
RUN dotnet publish "QualificationService.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Run as a non-root user
USER $APP_UID

ENTRYPOINT ["dotnet", "QualificationService.dll"]