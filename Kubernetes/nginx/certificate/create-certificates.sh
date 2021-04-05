#!/bin/sh

set -eo pipefail

echo "create CA"
cfssl gencert -initca cacsr.json | cfssljson -bare ca -

echo "create Server"
cfssl gencert -ca=ca.pem -ca-key=ca-key.pem -config=caconfig.json -profile=server servercsr.json | cfssljson -bare server -
