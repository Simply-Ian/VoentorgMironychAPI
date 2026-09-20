from mcr.microsoft.com/dotnet/aspnet:10.0 as base
workdir /app
expose 8080

from mcr.microsoft.com/dotnet/sdk:10.0 as build
workdir /src

copy ["src/Services/Catalog/Catalog.API/Catalog.API.csproj", "/src/Services/Catalog/Catalog.API/"]
copy ["src/Services/Catalog/Catalog.Application/Catalog.Application.csproj", "/src/Services/Catalog/Catalog.Application/"]
copy ["src/Services/Catalog/Catalog.Domain/Catalog.Domain.csproj", "/src/Services/Catalog/Catalog.Domain/"]
copy ["src/Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "/src/Services/Catalog/Catalog.Infrastructure/"]
copy ["src/Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "/src/Services/Catalog/Catalog.Infrastructure/"]
copy ["src/Common/Common.csproj", "/src/Common/"]

run dotnet restore "/src/Services/Catalog/Catalog.API/Catalog.API.csproj"

copy /src .
run ls -alh "Services/Catalog/Catalog.API"
run dotnet build "Services/Catalog/Catalog.API/Catalog.API.csproj" --configuration Release -o /app/build

from build as publish
run dotnet publish "/src/Services/Catalog/Catalog.API/Catalog.API.csproj" --configuration Release -o /app/publish -p:UseAppHost=false

from base as final
workdir /app

copy --from=publish /app/publish .

entrypoint dotnet Catalog.API.dll