# VagaFlow — Primeiro teste do usuário zero (0.1-alpha)

**Estado:** prévia de integração, sem merge na `main`.  
**Data:** 12/11/2026  
**Objetivo:** cadastrar uma oportunidade real no Radar por empresa + link + texto, exibir e filtrar vagas salvas localmente. Sem busca automática, sem candidatura automática e sem informações sensíveis.

## 1. Instalar pré-requisitos

- Git
- SDK .NET **10**
- Docker + plugin Docker Compose
- Node.js **22.12+** (ou versão suportada pelo Angular 21)
- npm
- OpenSSL (somente para gerar senha local)

Conferir:

```bash
git --version
dotnet --version
docker compose version
node --version
npm --version
```

## 2. Clonar a prévia

```bash
git clone --branch integration/vf-01-alpha --single-branch https://github.com/hallanbrito/VagaFlow.git
cd VagaFlow
```

A branch `main` continua protegida do trabalho ainda em revisão. Use esta integração apenas em ambiente local de desenvolvimento.

## 3. Subir o banco e preparar o acesso da API

```bash
cp infra/.env.example infra/.env
sed -i "s|SUBSTITUA_POR_UMA_SENHA_FORTE|$(openssl rand -hex 24)|" infra/.env
docker compose --env-file infra/.env -f infra/compose.yml up -d --wait
```

Ainda na raiz do repositório, em **bash/zsh**:

```bash
set -a
source infra/.env
set +a
export ConnectionStrings__VagaFlow="Host=127.0.0.1;Port=${VAGAFLOW_DB_PORT:-55432};Database=vagaflow;Username=vagaflow;Password=${VAGAFLOW_DB_PASSWORD}"
```

Não cole essa string com senha em issues, chats públicos ou screenshots.

## 4. Executar migrações explicitamente

```bash
dotnet tool restore
dotnet ef database update \
  --project apps/api/src/VagaFlow.Api/VagaFlow.Api.csproj \
  --startup-project apps/api/src/VagaFlow.Api/VagaFlow.Api.csproj \
  --context VagaFlowDbContext
```

Não há migrações executadas automaticamente na inicialização.

## 5. Iniciar API

Ainda no terminal com `ConnectionStrings__VagaFlow` exportada:

```bash
dotnet run --project apps/api/src/VagaFlow.Api/VagaFlow.Api.csproj --launch-profile local
```

Em outro terminal, confira:

```bash
curl -i http://127.0.0.1:5050/health
```

Resposta: `200 OK` com `status=healthy`. Isso comprova apenas que a API está ativa, **não** que vagas foram importadas.

## 6. Iniciar o Angular

Abra outro terminal na mesma pasta do repositório:

```bash
cd apps/web
npm install
npm start
```

Abra **http://127.0.0.1:4200/radar**.

- Informe empresa, cargo, link HTTPS de origem, área, modalidade e, se quiser, a descrição colada do anúncio.
- Insira data de publicação apenas se a conferiu no anúncio; caso contrário, deixe em branco.
- Salve a vaga e confira a lista; altere filtros para Teresina/PI, remoto, área e recência.
- Use **Abrir vaga original** para continuar no portal externo. **Abrir não equivale a candidatar-se.**

## 7. Testes

```bash
dotnet test apps/api/tests/VagaFlow.Api.Tests/VagaFlow.Api.Tests.csproj
cd apps/web && npm test && npm run build
```

Os testes da API com PostgreSQL real são executados quando `VAGAFLOW_TEST_DB_CONNECTION` está definida; o GitHub Actions configura esse banco descartável automaticamente. Para teste local com banco, exporte `VAGAFLOW_TEST_DB_CONNECTION="$ConnectionStrings__VagaFlow"` e rode os testes. **Não execute dados sintéticos de teste contra um banco de produção.**

## 8. Limitações atuais

- Não pesquisa vagas na Internet automaticamente; só mostra anúncios adicionados manualmente.
- Não comprova automaticamente se empresa, link, data ou anúncio são legítimos.
- O formulário não extrai os campos automaticamente do texto colado; esses campos precisam ser preenchidos/revisados.
- A data de coleta `firstSeenAtUtc` nunca substitui a publicação `publishedAtUtc`.
- Vagas remotas não são automaticamente consideradas elegíveis para candidatos no Brasil.
- Currículos, score de compatibilidade e status de candidatura ainda não estão implementados.
- A API não possui autenticação e foi concebida para `127.0.0.1`; **não exponha** o backend ou o PostgreSQL à Internet.
- Versões e dependências estão em revisão por PR; teste funcional no navegador do usuário zero ainda é necessário.

## 9. Parar sem excluir o banco

Após encerrar API e Angular com `Ctrl+C`:

```bash
docker compose --env-file infra/.env -f infra/compose.yml down
```

Isso preserva o volume. **Não use `down -v`** sem backup.

## PRs que compõem esta prévia

- Fundação: #2
- API: #5
- Angular shell: #6
- PostgreSQL: #8
- Modelo de vagas: #11
- Cadastro manual: #12
- Tela Radar: #14

Branch de integração não substitui revisão nem merge individual desses PRs.
