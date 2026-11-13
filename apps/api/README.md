# VagaFlow API — VF-002

API minimal em **ASP.NET Core / .NET 10**, executada localmente. O endpoint `GET /health` está implementado; currículos, vagas e persistência ficam para issues seguintes.

## Pré-requisitos

- SDK .NET 10 (`dotnet --version`)
- Bash, curl e Python 3 para o smoke test opcional

## Executar no CachyOS/Linux

Na raiz do repositório:

```bash
dotnet restore apps/api/tests/VagaFlow.Api.Tests/VagaFlow.Api.Tests.csproj
dotnet run --project apps/api/src/VagaFlow.Api/VagaFlow.Api.csproj --launch-profile local
```

Em outro terminal:

```bash
curl -i http://127.0.0.1:5050/health
bash scripts/smoke-api.sh
dotnet test apps/api/tests/VagaFlow.Api.Tests/VagaFlow.Api.Tests.csproj
```

Resposta esperada (`HTTP 200`):

```json
{"status":"healthy","service":"VagaFlow.Api","version":"0.1.0-alpha"}
```

- Perfil `local` atende apenas em `127.0.0.1:5050`. Não exponha a API na Internet sem autenticação e revisão de segurança.
- Testes usam `WebApplicationFactory`: não dependem de PostgreSQL, Docker ou servidores reais.
- Não versionar currículos pessoais, laudos ou secrets. Veja `.gitignore`.

## Próximas issues

- VF-004: banco PostgreSQL + EF Core
- VF-005: perfil mestre
- VF-009/010: vagas e importação rápida
