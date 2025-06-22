FROM mcr.microsoft.com/dotnet/aspnet:9.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 5045

ENV ASPNETCORE_URLS=http://+:5045

FROM mcr.microsoft.com/dotnet/sdk:9.0-nanoserver-1809 AS build
ARG configuration=Release
WORKDIR /src
COPY ["PersonApi.csproj", "./"]
RUN dotnet restore "PersonApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "PersonApi.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "PersonApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PersonApi.dll"]
