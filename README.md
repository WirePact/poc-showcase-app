# PoC Showcase Application

This repository contains a showcase application to demonstrate
the use of the distributed authentication mesh (WirePact) as well as
having some application during development.

The application is split into three parts:

- Frontend: A basic application that authenticates with [Zitadel](https://zitadel.ch)
- Modern-Service: The service that is called by the frontend with OIDC credentials
- Legacy-Service: A service that only is capable of Basic authentication

The deployment of the application is built for any Kubernetes
instance. If you need an API gateway, uncomment the according
section in the root `kustomization.yaml` file (i.e. ambassador
or nginx). By default, Ambassador will be used. Ambassador
also provides self signed certificates to run the frontend
with TLS to mitigate same-site cookie issues during authentication.

The showcase application will - for simplicities sake - call
the "modern service" with an internal kubernetes route (i.e. the
service DNS name). With this setup, only one ingress / mapping is
needed.

## Installation

To run the showcase application, follow these steps:

1. Provide a Kubernetes cluster (_hint_: not tested with "online" clusters)
   - Create one with `minikube`
   - Use the one of `Docker Desktop`
   - Use any other local Kubernetes creator
2. Use one of following `hosts` (`/etc/hosts` on \*nix and `C:\Windows\System32\drivers\etc\hosts` on windows)
   entries in your system to point to your cluster IP (only these urls are registered
   in the OIDC application for the frontend, so it has to be one of these url):
   - kubernetes.docker.internal
   - kubernetes.local
   - localhost
3. Install the API gateway
   - Ambassador (recommended)
     - Use the `install-ambassador.sh` script under `Kubernetes/ambassador`
     - Ambassador is installed
   - nginx
     - Use the `install-nginx.sh` script under `Kubernetes/nginx`
     - If you want to create your own certificates, install `cfssl` locally and
       use the `create-certificates.sh` script in`Kubernetes/nginx/certificate`
     - Uncomment `nginx` and comment `ambassador` out in the root `kustomization.yaml` in the `Kubernetes` folder
4. Configure if the showcase app should use the auth mesh or not (not yet available...)
   by setting the corresponding setting (`USE_WIREPACT=...`) the root `kustomization.yaml`
5. Use `kustomize build | kubectl apply -f -` to build the kustomization config
   and apply it to your cluster
6. Open the chosen url from point 2 and you should the the frontend application

_Hint_: With `kubectl logs -l app.kubernetes.io/part-of=wirepact-poc-showcase -f`
you can show and follow all logs from the showcase pods at once while you navigate through
the application. There are some log points that describe which credentials have arrived.
