FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

FROM  mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ENV NUGET_PACKAGES=/root/.nuget/packages
COPY ["src/web/dbs.blog/dbs.blog.csproj", "src/web/dbs.blog/"]
COPY ["src/web/dbs.domain/dbs.domain.csproj", "src/web/dbs.domain/"]
COPY ["src/web/dbs.infra/dbs.infra.csproj", "src/web/dbs.infra/"]
COPY ["src/core/dbs.core/dbs.core.csproj", "src/core/dbs.core/"]
RUN dotnet restore "src/web/dbs.blog/dbs.blog.csproj"
COPY . .
WORKDIR "/src/src/web/dbs.blog"
RUN dotnet publish "dbs.blog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "dbs.blog.dll"]