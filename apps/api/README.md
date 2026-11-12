# VagaFlow API — VF-002

API minimal em **ASP.NET Core / .NET 10**, executada localmente. O endpoint `GET /health` está implementado; VF-004 adiciona infraestrutura PostgreSQL, EF Core e uma migração técnica de teste. As entidades de vagas e candidatos ainda não existem.

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

- VF-004: PostgreSQL, EF Core, migração inicial e teste de persistência (branch de implementação)
- VF-005: perfil mestre
- VF-009/010: vagas e importação rápida

## Persistência PostgreSQL (VF-004)

Siga [infra/README.md](../../../infra/README.md) para iniciar o banco, configurar `ConnectionStrings__VagaFlow`, aplicar e reverter migrações. A API não roda migrações automaticamente; `/health` funciona sem banco e não afirma que o banco está pronto.

No CI, `DatabaseIntegrationTests` recebe `VAGAFLOW_TEST_DB_CONNECTION` apontando a um serviço PostgreSQL descartável e valida migração, gravação e leitura. Sem essa variável, o teste de banco é ignorado para não exigir Docker no desenvolvimento do endpoint `/health`.
