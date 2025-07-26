# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./
RUN ls -la /app               # ✅ Debug: verify files copied
RUN dotnet restore
RUN dotnet publish -c Release --no-restore -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./
COPY ./HBITS.db /app/HBITS.db    # ✅ This is your DB copy step

# 🧪 Debug: confirm HBITS.db and other files exist
RUN ls -la /                     # Check root contents
RUN ls -la /app                  # Check your app directory
RUN ls -la /app/wwwroot/data || echo "No wwwroot/data directory"

EXPOSE 80
ENTRYPOINT ["dotnet", "HBITSExplorer.dll"]
