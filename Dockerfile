# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

# Render pasa automáticamente estas variables
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
ENV POSTGRES_CONNECTION_STRING=${POSTGRES_CONNECTION_STRING}

ENTRYPOINT ["dotnet", "CongresoApi.dll"]
