# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet publish -c Release -o /app/build

# Runtime Stage
FROM nginx:alpine
COPY --from=build /app/build/wwwroot /usr/share/nginx/html
