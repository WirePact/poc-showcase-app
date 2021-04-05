﻿### Build
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /app

COPY . ./

RUN dotnet publish -o artifacts -c Release Frontend

### Deploy
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final

LABEL org.opencontainers.image.source=https://github.com/buehler/distributed-authentication-mesh

EXPOSE 80
ENV ASPNETCORE_URLS=http://0.0.0.0:80 \
    API_BASE_URL=http://kubernetes.docker.internal/api

WORKDIR /app

COPY --from=build /app/artifacts .

RUN ln -sf /usr/share/zoneinfo/Europe/Zurich /etc/localtime && \
  dpkg-reconfigure -f noninteractive tzdata

ENTRYPOINT [ "dotnet" ]
CMD [ "Frontend.dll" ]
