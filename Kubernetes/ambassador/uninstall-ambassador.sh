#!/bin/sh

set -eo pipefail

kubectl delete ns ambassador && \
kubectl delete -f https://www.getambassador.io/yaml/aes-crds.yaml
