#!/usr/bin/env bash
set -euo pipefail

url="${1:-http://127.0.0.1:5050/health}"
payload="$(curl --fail --silent --show-error --max-time 10 "$url")"
python3 - "$payload" <<'PY'
import json
import sys

data = json.loads(sys.argv[1])
assert data.get("status") == "healthy", f"Status inesperado: {data!r}"
assert data.get("service") == "VagaFlow.Api", f"Serviço inesperado: {data!r}"
print("OK /health: API do VagaFlow respondendo corretamente")
PY
