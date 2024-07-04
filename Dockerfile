FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
EXPOSE 8080
# Projeyi kopyala ve restore et
COPY . ./
RUN dotnet restore

# Projeyi yayınla
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Önceki aşamadan yayınladığımız dosyaları kopyala
COPY --from=build-env /app/out .

# Redis password environment variable
ENV REDIS_PASSWORD your-redis-password

# Replace placeholder in appsettings.json
RUN sed -i 's/\${REDIS_PASSWORD}/'"$REDIS_PASSWORD"'/g' appsettings.json
# Uygulamayı çalıştırmak için gerekli komutu belirt
ENTRYPOINT ["dotnet", "RedisApp.dll"]