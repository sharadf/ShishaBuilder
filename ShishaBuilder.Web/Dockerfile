FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ShishaBuilder.sln ./
COPY ShishaBuilder.Core ./ShishaBuilder.Core
COPY ShishaBuilder.Business ./ShishaBuilder.Business
COPY ShishaBuilder.Web ./ShishaBuilder.Web


RUN dotnet publish ./ShishaBuilder.Web/ShishaBuilder.Web.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ShishaBuilder.Web.dll"]