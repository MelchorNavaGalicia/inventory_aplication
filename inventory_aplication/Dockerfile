# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copiar solo el csproj y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out ./

# Exponer puerto
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

# Comando para ejecutar la API
ENTRYPOINT ["dotnet", "inventory_aplication.dll"]

