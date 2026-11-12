# VagaFlow — pacote inicial de planejamento

**Status:** documentação planejada, software ainda não implementado.  
**Data:** 12/11/2026  
**Prioridade:** recolocação profissional do usuário zero.

## Documentação

1. [`docs/01-VISAO-DO-PRODUTO.md`](docs/01-VISAO-DO-PRODUTO.md) — problema, públicos, proposta de valor, métricas e regras do usuário zero.
2. [`docs/02-ESCOPO-MVP-0.1.md`](docs/02-ESCOPO-MVP-0.1.md) — funcionalidades incluídas/excluídas, fluxos e Definition of Done.
3. [`docs/03-ARQUITETURA.md`](docs/03-ARQUITETURA.md) — arquitetura, contratos, dados, scoring, APIs e segurança.
4. [`docs/04-BACKLOG-INICIAL.md`](docs/04-BACKLOG-INICIAL.md) — 24 issues propostas com prioridades, dependências e aceite.
5. [`planning/backlog.csv`](planning/backlog.csv) — backlog editável em planilha ou GitHub Projects (ajustes de importação podem ser necessários).

## Primeira entrega utilizável — 0.1-alpha

Cadastrar perfil mínimo → colar uma vaga real e seu link → visualizar no radar → abrir o portal e registrar resultado. O gerador de PDF e as integrações automáticas não bloqueiam essa primeira entrega.

## Resultado esperado do MVP 0.1

**Perfil e currículos → Radar real com fontes rastreáveis → Match explicável → Link de candidatura → Registro manual do envio.**

A revisão de escopo registrada em `docs/decisions/0003-usuario-zero-velocidade-e-pdf.md` detalha importação rápida de vagas locais, fallback de currículo e avaliação de QuestPDF.

## Primeiros passos

1. Revisar documentos e regras de busca (a fundação do GitHub já foi iniciada).
2. Acompanhar a [issue VF-001](https://github.com/hallanbrito/VagaFlow/issues/1) e revisar a fundação em PR.
3. Abrir issue `VF-001` e implementar estrutura mínima.
4. Validar fluxo com dados sintéticos; usar dados reais apenas no banco e diretórios locais privados.
5. Expandir para integrações permitidas somente após revisão de termos e confiabilidade.

> Não adicione currículos reais, laudos, e-mails pessoais, chaves ou tokens ao repositório público.