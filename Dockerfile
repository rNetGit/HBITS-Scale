# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release --no-restore -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./
COPY ./HBITS.db /app/HBITS.db    # ✅ Copy DB into runtime container

EXPOSE 80
ENTRYPOINT ["dotnet", "HBITSExplorer.dll"]
