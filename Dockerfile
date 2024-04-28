FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Projeyi kopyala ve restore et
COPY . ./
RUN dotnet restore

# Projeyi yayınla
RUN for project in $(find . -name '*.csproj'); do \
    dotnet publish -c Release -o /app/out/$(dirname $project)/publish $project; \
done

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Önceki aşamadan yayınladığımız dosyaları kopyala
COPY --from=build-env /app/out .

# Uygulamayı çalıştırmak için gerekli komutu belirt
ENTRYPOINT ["dotnet", "RedisApp.dll"]