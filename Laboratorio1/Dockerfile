FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copiar el archivo de proyecto y restaurar dependencias
COPY ["personapi-dotnet.csproj", "./"]
RUN dotnet restore "./personapi-dotnet.csproj"

# Copiar el resto del código y compilar
COPY . .
RUN dotnet publish "personapi-dotnet.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Construir la imagen final
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Exponer el puerto 80 (puerto por defecto en contenedores ASP.NET)
EXPOSE 80

ENTRYPOINT ["dotnet", "personapi-dotnet.dll"]
