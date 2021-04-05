### Build
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /app

COPY . ./

RUN dotnet publish -o artifacts -c Release Legacy-Service

### Deploy
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final

LABEL org.opencontainers.image.source=https://github.com/WirePact/poc-showcase-app

EXPOSE 80
ENV ASPNETCORE_URLS=http://0.0.0.0:80

WORKDIR /app

COPY --from=build /app/artifacts .

RUN ln -sf /usr/share/zoneinfo/Europe/Zurich /etc/localtime && \
  dpkg-reconfigure -f noninteractive tzdata

ENTRYPOINT [ "dotnet" ]
CMD [ "Legacy-Service.dll" ]
