# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY *.sln .
COPY api/*.csproj api/

RUN dotnet restore
COPY . .

# publish
FROM build AS publish
WORKDIR /src/api
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .

# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet api.dll