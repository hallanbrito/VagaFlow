# VagaFlow — Escopo fechado do MVP 0.1

**Data:** 12/11/2026  
**Estado:** Especificação inicial — nenhum item significa implementação pronta.

## 1. Resultado mínimo útil

O usuário zero deve conseguir: (a) manter um perfil mestre verdadeiro; (b) gerar/selecionar seus quatro currículos; (c) visualizar vagas reais com empresa e link; (d) filtrar o que serve à sua busca; (e) entender a recomendação; (f) registrar quando se candidatou, sem duplicar inscrições.

**Não bloquear o radar pelo gerador de currículos:** permitir cadastrar manualmente dados mínimos e anexar as quatro versões de PDF já existentes, enquanto o gerador evolui. A ordem prioriza empregabilidade imediata.

## 2. Incluído (MUST)

### A. Perfil e currículos

- CRUD de perfil com experiência, formação, projetos, habilidades, preferências, localização e remuneração de referência.
- Cadastro das quatro variantes de currículo; associação com `area = TI | LOGISTICA` e `publico = GERAL | PCD`.
- Anexar PDFs existentes **sem extração automática** no 0.1; conteúdo estruturado revisado manualmente.
- Gerar PDF ATS simples a partir do perfil mestre com HTML/CSS + renderização; links funcionais, texto selecionável, seções sem colunas complexas.
- Pré-visualização e confirmação dos fatos antes de exportar; controles explícitos de inclusão de informações PcD.

### B. Fontes e vagas

- Importação manual de vaga por formulário: link HTTPS, empresa, cargo, localidade, modalidade, descrição, fonte e data publicada (quando comprovada).
- Ao menos **um adaptador funcional** de leitura para vagas publicadas (Greenhouse Job Board API como primeira opção); Lever como próximo adaptador dentro do backlog P1 se aprovado.
- Configurar fontes por empresa (`board_token` / identificador) e não prometer busca global.
- Normalização para um contrato único; desduplicação por `(fonte, id externo)` e por URL canônica quando possível.
- Campos independentes: `published_at`, `source_updated_at`, `first_seen_at`, `last_checked_at` e status de verificação.
- Empresa identificada e URL de candidatura obrigatórias para aparecer como vaga elegível.
- Filtros: área, palavras-chave, remota Brasil ou Teresina presencial/híbrido, data comprovada desde 01/11/2026, vagas PcD e status já candidatado.
- Fila de revisão para vagas com data desconhecida, localização ambígua ou empresa não confirmada.

### C. Matching e candidaturas

- Pontuação explicável baseada em critérios com pesos configuráveis; separar incompatibilidades confirmadas de campos desconhecidos.
- Sugestão da variante de currículo com escolha final do usuário.
- Registro manual de candidatura, status, data, link externo e observações; validação contra duplicatas.
- Tela de radar, detalhe da vaga e painel de candidaturas.

### D. Qualidade e operação

- Aplicação Angular + API .NET + PostgreSQL em execução local com Docker Compose para o banco.
- Logs sem dados sensíveis; testes de regras, filtros e desduplicação; documentação de setup e variáveis.
- Navegação básica por teclado, rótulos adequados, contrastes legíveis e mensagens de erro acionáveis.

## 3. Excluído (WON'T no 0.1)

- Inscrição automática, automação de formulários de terceiros ou bypass de CAPTCHA.
- Crawling indiscriminado de portais ou coleta de conteúdo contra seus termos.
- Monitoramento 24h/alertas push; agendamentos remotos.
- Login social, multi-tenant, perfis familiares e cobrança.
- LLM obrigatório, embeddings, vector DB, microserviços, filas distribuídas e Kubernetes.
- Parser confiável universal de PDF, OCR e importação completa de perfis de portais.
- Leitura da caixa de e-mail e detecção automática de aprovação/reprovação.
- Armazenamento de laudo médico, CID ou outras informações clínicas.

## 4. Fluxos e telas

| Tela | Interação mais importante | Critério de conclusão |
|---|---|---|
| Meu perfil | Editar dados e preferências | Persistência e validação; usuário confirma dados |
| Meus currículos | Anexar/gerar/baixar quatro variantes | PDF correto, texto selecionável, download funcional |
| Radar | Buscar, filtrar, ordenar, revisar data/fonte | Só vagas elegíveis no resultado principal |
| Detalhe da vaga | Ver descrição, requisitos, match, currículo indicado e link | Justificativa transparente, link externo seguro |
| Candidaturas | Salvar, marcar enviada, atualizar etapas | Sem envio presumido; histórico consistente |
| Fontes (local/admin) | Cadastrar empresa/board e executar sincronização | Registro de erro/status da fonte |

## 5. Histórias críticas e regras de aceite

- **US-01 — Perfil mestre:** Dado um usuário com múltiplas experiências, quando alterar uma informação, ela passa a integrar o perfil vigente, sem alterar silenciosamente PDFs antigos.
- **US-02 — Quatro currículos:** Dado o perfil, ao gerar um PDF, a variante selecionada inclui apenas informação confirmada, conserva URL clicável e não inclui informação PcD por padrão na variante Geral.
- **US-03 — Radar confiável:** Dada vaga sem empresa confiável ou sem URL externa, ela não é listada como recomendação pronta.
- **US-04 — Recência comprovada:** Uma vaga coletada hoje, mas sem data de publicação, não entra no filtro `publicada desde 01/11/2026`.
- **US-05 — Geografia:** Vaga presencial fora de Teresina é classificada como incompatível para o usuário zero; remota só é elegível quando aceitar Brasil ou quando isso for confirmável.
- **US-06 — Match:** Cada pontuação exibe fatores positivos, ausentes e não informados, com indicação de que não mede chance de contratação.
- **US-07 — Currículo PcD:** Exclusiva/afirmativa PcD sugere a variante PcD apropriada; vaga geral sugere versão Geral; usuário pode trocar antes de usar.
- **US-08 — Candidaturas:** Abrir o link externo não altera status para `enviada`; duplicata detectada informa aplicação já existente.
- **US-09 — Fonte:** Falha de API não apaga vagas anteriores, e dados vencidos são assinalados como não verificados recentemente.
- **US-10 — Segurança:** O backend só é exposto em loopback no desenvolvimento, arquivos sensíveis não entram no Git, e chamadas externas não recebem dados pessoais do candidato.

## 6. Definição de pronto do MVP

Todos os itens a seguir precisam estar demonstráveis, não apenas documentados:

- [ ] Abrir aplicação e API localmente a partir de `README` reproduzível.
- [ ] Cadastrar perfil verdadeiro, preferências e quatro PDFs/variantes.
- [ ] Gerar e baixar ao menos um PDF ATS funcional com conteúdo revisado.
- [ ] Inserir uma vaga manualmente e obter vagas reais de pelo menos uma fonte pública permitida.
- [ ] Exibir empresa, cargo, URL, fonte, modalidade e status de recência sem inventar datas.
- [ ] Aplicar filtro específico do usuário zero e rejeitar duplicatas.
- [ ] Obter justificativa de compatibilidade e recomendação de currículo.
- [ ] Registrar uma candidatura real apenas após confirmação explícita do usuário.
- [ ] Testes automatizados verdes para regras críticas, com evidência.
- [ ] Nenhum segredo, currículo real ou dado sensível versionado no repositório.

## 7. Sequência de validação com o usuário zero

1. Preencher preferências com dados reais, sem dados clínicos.
2. Cadastrar as quatro variantes e conferir os PDFs.
3. Cadastrar pelo menos duas fontes confiáveis de empresa; testar a primeira integração.
4. Importar manualmente links relevantes de portais não integrados, com revisão.
5. Testar filtro estrito para oportunidades desde 01/11/2026.
6. Abrir vaga, conferir adequação e currículo sugerido.
7. Candidatar-se no site oficial e registrar `enviada` somente depois.
8. Coletar feedback do tempo economizado e falhas reais do fluxo.

## 8. Pontos que não podem ser confundidos

- **Coleta ≠ publicação:** `first_seen_at` não substitui `published_at`.
- **Link aberto ≠ candidatura enviada**.
- **Nota de match ≠ previsão estatística de contratação**.
- **Vaga PcD bem-vindos ≠ vaga exclusiva PcD**.
- **Site de carreira público ≠ permissão irrestrita de scraping**.

---
