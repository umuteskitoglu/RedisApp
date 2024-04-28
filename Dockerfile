FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Projeyi kopyala ve restore et
COPY . ./
RUN dotnet restore

# Her bir projeyi yayınla ve ayrı bir çıkış dizinine yerleştir
RUN for project in $(find . -name '*.csproj'); do \
    project_dir=$(dirname $project); \
    publish_dir=/app/out/$(basename $project_dir)/publish; \
    mkdir -p $publish_dir; \
    dotnet publish -c Release -o $publish_dir $project; \
done

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Önceki aşamadan yayınladığımız dosyaları kopyala
COPY --from=build-env /app/out .

# Uygulamayı çalıştırmak için gerekli komutu belirt
ENTRYPOINT ["dotnet", "RedisApp.dll"]