# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ShishaBuilder.Web/ShishaBuilder.Web.csproj ShishaBuilder.Web/
COPY Core/*.csproj Core/
COPY Business/*.csproj Business/
RUN dotnet restore ShishaBuilder.Web/ShishaBuilder.Web.csproj

COPY . .
WORKDIR /src/ShishaBuilder.Web
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app 
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ShishaBuilder.Web.dll"]
