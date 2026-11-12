# Infraestrutura local — VF-004

PostgreSQL 17 em Docker Compose, **restrito a `127.0.0.1`**, com volume para persistência. Não é um serviço hospedado e não contém dados reais versionados.

## 1. Iniciar

Pré-requisitos: Docker Engine e plugin Compose, SDK .NET 10.

Na raiz do repositório:

```bash
cp infra/.env.example infra/.env
# Edite infra/.env e substitua o valor VAGAFLOW_DB_PASSWORD por senha forte.
# Exemplo para gerar: openssl rand -hex 24
docker compose --env-file infra/.env -f infra/compose.yml up -d
docker compose --env-file infra/.env -f infra/compose.yml ps
```

O banco escuta somente no host `127.0.0.1:55432` (ou porta configurada). O arquivo `infra/.env` não pode ser commitado. **Nunca use a senha de exemplo como senha real.**

## 2. Variável de conexão da API

```bash
set -a
source infra/.env
set +a
export ConnectionStrings__VagaFlow="Host=127.0.0.1;Port=${VAGAFLOW_DB_PORT:-55432};Database=vagaflow;Username=vagaflow;Password=${VAGAFLOW_DB_PASSWORD}"
```

Essas variáveis existem apenas no terminal atual. Nunca coloque a string de conexão em código, README pessoal ou logs. Se a senha contiver caracteres especiais que atrapalhem uma connection string Npgsql, use uma senha hexadecimal (gerada com `openssl rand -hex 24`).

## 3. Aplicar migração

```bash
dotnet tool restore
dotnet restore apps/api/tests/VagaFlow.Api.Tests/VagaFlow.Api.Tests.csproj
dotnet ef database update \
  --project apps/api/src/VagaFlow.Api/VagaFlow.Api.csproj \
  --startup-project apps/api/src/VagaFlow.Api/VagaFlow.Api.csproj \
  --context VagaFlowDbContext
```

A migração `20261112190000_InitialInfrastructure` cria apenas a tabela técnica `app_settings` (sem conteúdo de candidatos). A tabela `__EFMigrationsHistory` fica sob controle do Entity Framework Core.

## 4. Testar

```bash
export VAGAFLOW_TEST_DB_CONNECTION="$ConnectionStrings__VagaFlow"
dotnet test apps/api/tests/VagaFlow.Api.Tests/VagaFlow.Api.Tests.csproj
```

Os testes com PostgreSQL usam somente chaves sintéticas temporárias e limpam seus registros.

## 5. Reverter migração (operações destrutivas)

> ATENÇÃO: reverter para `0` remove as tabelas criadas por migrações. Só execute em banco de teste vazio, sem dados relevantes.

```bash
dotnet ef database update 0 \
  --project apps/api/src/VagaFlow.Api/VagaFlow.Api.csproj \
  --startup-project apps/api/src/VagaFlow.Api/VagaFlow.Api.csproj \
  --context VagaFlowDbContext
```

## 6. Parar sem apagar dados

```bash
docker compose --env-file infra/.env -f infra/compose.yml down
```

Não use `down -v` sem backup: esse comando **apaga o volume** do banco.

## Limites e segurança

- A aplicação não executa migrações no `Program.cs`.
- `/health` verifica somente o processo API; não é teste de prontidão da base.
- Não exponha a porta do PostgreSQL nem a API para a Internet.
- Contas multiusuário, segregação de dados e autenticação serão adicionadas antes de qualquer publicação externa.
