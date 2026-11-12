# VagaFlow — Backlog inicial

**Versão:** backlog de implementação proposto · **Data:** 12/11/2026

## Convenções

- **P0:** necessário para o MVP 0.1; **P1:** valioso após o primeiro release; **P2:** posterior.
- **S/M/L:** tamanho relativo, não prazo. Priorizar issues pequenas, testáveis e revisáveis.
- **Status inicial:** todas as issues propostas, não iniciadas.
- **IDs VF-###:** identificadores internos de planejamento, **não** números reais de GitHub Issues.

## Ordem de execução recomendada

### S0 — Fundação & fluxo mínimo `0.1-alpha`
`VF-001`, `VF-002`, `VF-003`, `VF-004`, `VF-005`, `VF-006`, `VF-009`, `VF-010`, `VF-013`

### S1 — Currículos e matching
`VF-007`, `VF-008`, `VF-011`, `VF-014`, `VF-015`, `VF-016`

### S2 — Radar automatizado e candidaturas
`VF-012`, `VF-017`, `VF-018`

### S3 — Qualidade & release
`VF-019`, `VF-020`

### Após MVP
`VF-021`, `VF-022`, `VF-023`, `VF-024`

## Itens com critérios de aceite

| ID | Prioridade | Épico | Título | Dependências | Tamanho |
|---|---|---|---|---|---|
| VF-001 | P0 | Fundação | Inicializar monorepo, README e ADRs | - | S |
| VF-002 | P0 | Fundação | Criar backend .NET 10 e endpoint /health | VF-001 | S |
| VF-003 | P0 | Fundação | Criar Angular shell com navegação acessível | VF-001 | S |
| VF-004 | P0 | Fundação | Configurar PostgreSQL e EF Core Migrations | VF-002 | M |
| VF-005 | P0 | Perfil | Modelar perfil mestre, experiências, formação e skills | VF-004 | M |
| VF-006 | P0 | Perfil | Criar formulário de perfil e preferências do usuário zero | VF-003;VF-005 | M |
| VF-007 | P0 | Currículos | Cadastrar quatro variantes e anexar PDFs existentes | VF-005;VF-003 | M |
| VF-008 | P0 | Currículos | Criar template ATS em .NET com geração PDF (avaliar QuestPDF) | VF-005;VF-007 | M |
| VF-009 | P0 | Vagas | Modelar fontes, vagas, datas e estados de verificação | VF-004 | M |
| VF-010 | P0 | Vagas | Criar importação manual rápida (URL + texto bruto) e validação | VF-009 | S |
| VF-011 | P0 | Vagas | Implementar normalização e desduplicação | VF-009 | M |
| VF-012 | P0 | Vagas | Adicionar adaptador Greenhouse e sync por empresa | VF-009;VF-011 | M |
| VF-013 | P0 | Vagas | Implementar lista Radar com filtros e revisão | VF-003;VF-010;VF-011 | M |
| VF-014 | P0 | Matching | Criar política de elegibilidade e testes | VF-005;VF-009 | M |
| VF-015 | P0 | Matching | Implementar score determinístico explicável | VF-014 | M |
| VF-016 | P0 | Matching | Sugerir variante de currículo por área e política PcD | VF-007;VF-015 | S |
| VF-017 | P0 | Candidaturas | Modelar candidatura, status e antirrepetição | VF-009;VF-005 | M |
| VF-018 | P0 | Candidaturas | Criar painel com atualização de status | VF-003;VF-017 | M |
| VF-019 | P0 | Qualidade | Escrever testes E2E da jornada usuário zero | VF-006;VF-008;VF-013;VF-016;VF-018 | M |
| VF-020 | P0 | Qualidade | Auditar segurança local, documentação e release 0.1 | VF-019;VF-012 | M |
| VF-021 | P1 | Fontes | Avaliar e implementar adaptador Lever | VF-012 | M |
| VF-022 | P1 | Produto | Instrumentar feedback de qualidade do resultado | VF-015;VF-018 | S |
| VF-023 | P1 | Pesquisa | Pesquisar novas fontes brasileiras permitidas | VF-012 | S |
| VF-024 | P2 | Produto | Planejar beta multiusuário e LGPD operacional | VF-020 | L |

## Critérios de aceite por issue

### VF-001 — Inicializar monorepo, README e ADRs
- **Prioridade:** P0 | **Tamanho:** S | **Dependências:** -
- **Aceite:** Estrutura documentada; .gitignore bloqueia .env/PDFs/dumps; build de exemplo reproduzível.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-002 — Criar backend .NET 10 e endpoint /health
- **Prioridade:** P0 | **Tamanho:** S | **Dependências:** VF-001
- **Aceite:** API sobe em loopback; /health responde 200; teste básico verde.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-003 — Criar Angular shell com navegação acessível
- **Prioridade:** P0 | **Tamanho:** S | **Dependências:** VF-001
- **Aceite:** Rotas Perfil, Currículos, Radar e Candidaturas; navegação por teclado.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-004 — Configurar PostgreSQL e EF Core Migrations
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-002
- **Aceite:** Compose local inicia banco; migrations aplicáveis e reversíveis.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-005 — Modelar perfil mestre, experiências, formação e skills
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-004
- **Aceite:** Entidades e DTOs com validação; CRUD testado; não incluir dados médicos.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-006 — Criar formulário de perfil e preferências do usuário zero
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-003;VF-005
- **Aceite:** Áreas/localidades/modalidades/pretensão editáveis; persistência confiável.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-007 — Cadastrar quatro variantes e anexar PDFs existentes
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-005;VF-003
- **Aceite:** 4 variantes com metadados; anexo validado; escolha Geral/PcD explícita.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-008 — Criar template ATS HTML com geração PDF
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-005;VF-007
- **Aceite:** PDF textual com links e conteúdo confirmado; prévia e download.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-009 — Modelar fontes, vagas, datas e estados de verificação
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-004
- **Aceite:** published_at separado de first_seen_at; empresa+URL obrigatórias para elegibilidade.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-010 — Criar importação manual de vaga e validação de URL
- **Prioridade:** P0 | **Tamanho:** S | **Dependências:** VF-009
- **Aceite:** Usuário informa empresa/cargo/URL; nenhum fetch arbitrário; rejeita inválidos.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-011 — Implementar normalização e desduplicação
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-009
- **Aceite:** Fonte+ID ou URL canônica únicas; testes de duplicidade.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-012 — Adicionar adaptador Greenhouse e sync por empresa
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-009;VF-011
- **Aceite:** GET oficial por board token configurado, backoff/timeouts, import real demonstrado.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-013 — Implementar lista Radar com filtros e revisão
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-003;VF-010;VF-011
- **Aceite:** Teresina/remoto BR/áreas/01-11; datas ausentes em revisão; URL externa funcional.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-014 — Criar política de elegibilidade e testes
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-005;VF-009
- **Aceite:** 3 estados; nenhuma data ou modalidade inventada; cenários do usuário zero cobertos.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-015 — Implementar score determinístico explicável
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-014
- **Aceite:** Met/gap/unknown e cobertura exibidos; nota não interpretada como chance.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-016 — Sugerir variante de currículo por área e política PcD
- **Prioridade:** P0 | **Tamanho:** S | **Dependências:** VF-007;VF-015
- **Aceite:** Exclusiva PcD sugere PcD; geral sugere Geral; usuário pode trocar.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-017 — Modelar candidatura, status e antirrepetição
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-009;VF-005
- **Aceite:** Status enviada só manualmente; unique perfil/vaga; histórico de datas.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-018 — Criar painel com atualização de status
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-003;VF-017
- **Aceite:** Lista salvas/enviadas/entrevista; alerta de duplicatas; próximo passo visível.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-019 — Escrever testes E2E da jornada usuário zero
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-006;VF-008;VF-013;VF-016;VF-018
- **Aceite:** Perfil -> PDF -> radar -> match -> link -> status registrado em cenário sintético.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-020 — Auditar segurança local, documentação e release 0.1
- **Prioridade:** P0 | **Tamanho:** M | **Dependências:** VF-019;VF-012
- **Aceite:** Sem PII/versionamento, backend loopback, README reproduzível, checklist DoD completo.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-021 — Avaliar e implementar adaptador Lever
- **Prioridade:** P1 | **Tamanho:** M | **Dependências:** VF-012
- **Aceite:** Termos revisados; empresa configurável; contrato normalizado e testado.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-022 — Instrumentar feedback de qualidade do resultado
- **Prioridade:** P1 | **Tamanho:** S | **Dependências:** VF-015;VF-018
- **Aceite:** Usuário marca recomendação útil/inútil com comentário; sem telemetria oculta.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-023 — Pesquisar novas fontes brasileiras permitidas
- **Prioridade:** P1 | **Tamanho:** S | **Dependências:** VF-012
- **Aceite:** Matriz cobertura/permissões/custos; sem integração não autorizada.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

### VF-024 — Planejar beta multiusuário e LGPD operacional
- **Prioridade:** P2 | **Tamanho:** L | **Dependências:** VF-020
- **Aceite:** Threat model, autenticação, isolamento, exclusão/exportação e base legal revisados.
- **Definição geral de pronto:** código revisável, testes pertinentes, README/API atualizados se houver mudança de contrato, nenhuma PII real em commits.

## Primeira issue a executar

**VF-001 — Inicializar monorepo, README e ADRs.** Recomenda-se abrir o repositório com arquivos documentais, depois fazer VF-002 e VF-003 em mudanças pequenas. Não criar todas as issues no GitHub sem revisar as dependências; o CSV é uma sugestão de importação/gerenciamento.

## Riscos e antiescopo

- Não implementar scraping indiscriminado ou submissão automática no 0.1.
- Não transformar o score em estimativa de aprovação; exibir razão e cobertura da evidência.
- Não bloquear o radar por integração de IA, contas pagas ou parser complexo de PDF.
- Não aceitar dados sensíveis em fixtures versionadas.

## Ordem revista após revisão do usuário zero — 12/11/2026

**`0.1-alpha` (uso diário o quanto antes):** VF-001 → VF-002/VF-003 → VF-004 → VF-009/VF-005 → VF-010 (colagem rápida) → VF-013 (Radar) → VF-017/VF-018 (histórico). VF-006 pode evoluir em paralelo como formulário simples.

**Depois do primeiro uso real:** VF-007 (anexar quatro currículos), VF-014/VF-015/VF-016 (elegibilidade, score, fallback de currículo), VF-011 (deduplicação robusta), VF-008 (spike QuestPDF e PDF ATS), VF-012 (Greenhouse). As dependências internas continuam obrigatórias; a sequência acima indica **valor**, não substitui o grafo de dependências.

**Regra para VF-010:** aceitar colar descrição bruta e link; sugerir dados sem gravar valores inferidos como fatos até a confirmação do usuário; deixar empresa e URL obrigatórias para vaga recomendada.

**Regra para VF-016:** áreas ADMINISTRACAO e ATENDIMENTO não equivalem automaticamente a LOGISTICA. Se houver evidência de habilidades transferíveis, indicar variante de logística como opção sujeita à aprovação; sem evidência, exigir escolha manual; PcD depende sempre de seleção explícita.

**Regra para VF-008:** avaliar QuestPDF sob a licença vigente antes de incorporar ao produto e testar PDF extraível, links e ordem de leitura; não tornar o radar dependente desse recurso.

**Regra para VF-012:** Greenhouse é complementar ao mercado local; não atrasar `0.1-alpha` por falta de vagas de Teresina na fonte.
