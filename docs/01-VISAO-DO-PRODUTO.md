# VagaFlow — Visão do Produto

**Versão:** 0.1 (documento inicial)  
**Data:** 12/11/2026  
**Status:** Proposta de implementação, sem funcionalidades implementadas  
**Patrocinador / usuário zero:** candidato em Teresina–PI, em transição de logística para tecnologia.  
**Prioridade organizacional:** #1.

## 1. Problema

Candidatos gastam muitas horas alternando entre sites de emprego, reescrevendo dados profissionais, escolhendo o currículo adequado, conferindo vagas repetidas ou desatualizadas e tentando acompanhar em quais empresas já se candidataram. Esses problemas se intensificam quando a mesma pessoa busca em múltiplas áreas profissionais.

**Problema imediato do usuário zero:** encontrar vagas reais e recentes adequadas a TI, logística, administração e call center; priorizar presencial/híbrido em Teresina–PI e remoto em território brasileiro; escolher corretamente entre currículos gerais e PcD; registrar e acompanhar candidaturas sem duplicações.

## 2. Visão

> Transformar uma trajetória profissional em oportunidades verificáveis: organizar perfil e currículos, encontrar vagas compatíveis, explicar recomendações e acompanhar candidaturas com transparência e controle humano.

**Elevator pitch:** VagaFlow é um copiloto de empregabilidade que reúne currículo mestre, versões ATS, radar de vagas com fontes verificáveis e organização de candidaturas. Começa resolvendo o problema de um usuário real e evolui por validação incremental.

## 3. Público-alvo e persona inicial

- **Usuário zero:** brasileiro adulto, formado em logística, graduando em Engenharia de Software, com projetos técnicos e histórico profissional; procura recolocação em várias áreas sem perder a precisão do currículo.
- **Próximos públicos (não no MVP 0.1):** estudantes buscando estágio, profissionais em transição, candidatos PcD, trabalhadores operacionais e administrativos, pessoas procurando primeiro emprego.
- **Valor inclusivo:** linguagem simples, navegação por teclado, texto legível, feedback de erro claro e controle da divulgação de informações pessoais.

## 4. Proposta de valor

1. **Uma fonte de verdade:** perfil mestre editável, sem versões contraditórias dos dados.
2. **Currículos direcionados e verdadeiros:** gerar/exportar versões por área, com ATS legível, sem inventar qualificações.
3. **Oportunidades rastreáveis:** cada vaga mostra cargo, empresa identificada, fonte, URL de candidatura, modalidade, localidade, data da publicação quando comprovada e horário da última verificação.
4. **Compatibilidade explicável:** regras transparentes e possibilidade de revisão; a nota não representa chance de contratação.
5. **Candidatura sob controle:** histórico, próximos passos, currículo sugerido e status informado pelo usuário, sem marcar envio apenas por abrir um link.

## 5. Regras do usuário zero

- **Geografia:** presencial ou híbrido apenas em Teresina–PI; remoto para vagas que aceitem candidatos no Brasil. Outras localidades dependem de aprovação futura.
- **Áreas:** TI em geral; logística; administração; call center/atendimento. Não restringir TI apenas a vagas de desenvolvimento.
- **Recência inicial:** priorizar vagas publicadas a partir de **01/11/2026**, quando a data for verificável. Data da coleta não é data de publicação. Vagas sem data de publicação comprovada ficam numa fila separada de revisão, fora do filtro estrito de recência.
- **Empresa identificada:** não exibir como recomendação pronta uma vaga sem nome confiável da empresa.
- **Inscrição por e-mail:** destacar se o domínio é corporativo; endereços em provedores públicos (gmail.com, outlook.com, hotmail.com etc.) não são considerados canais corporativos válidos para o fluxo do usuário zero.
- **Pretensão salarial de referência:** R$ 2.591,00, editável e sem descartar automaticamente anúncios que não informem salário.
- **Quatro currículos:** Full Stack Geral; Full Stack PcD; Logística Geral; Logística PcD. Em vagas exclusivas/afirmativas PcD, sugerir variante PcD da área; caso contrário, preferir variante Geral, com escolha final do usuário. Autodeclarações devem ser sempre verdadeiras.
- **Sem duplicidades:** impedir ou sinalizar candidaturas à mesma vaga/mesma empresa e cargo, preservando o histórico.
- **Sem automação de envio no MVP:** nunca candidatar-se, anexar laudo ou enviar dados pessoais a empresas automaticamente.

## 6. Jornada central

1. Usuário importa ou cadastra seu currículo/perfil mestre e confere os fatos.
2. Sistema gera as quatro versões ATS e permite baixá-las.
3. Radar coleta oportunidades de fontes permitidas e/ou recebe URLs inseridas pelo usuário.
4. Sistema normaliza, desduplica, verifica qualidade dos dados e aplica filtros.
5. Usuário vê **por que** a vaga combina (ou não) e qual currículo recomenda-se usar.
6. Ao abrir o portal externo, o usuário se candidata manualmente e, quando confirmar o envio, registra o status.
7. Painel lembra quais vagas já foram analisadas e quais exigem ação.

## 7. Métricas e hipóteses

| Métrica | Como medir | Meta inicial de validação (hipótese, não promessa) |
|---|---|---|
| Tempo até primeira vaga aproveitável | A partir do perfil pronto até abrir link válido | Reduzir em comparação ao processo manual |
| Qualidade de links | Proporção de vagas exibidas com URL externa válida na última verificação | >= 90% na amostra verificada |
| Precisão de identidade | Vagas recomendadas com empresa identificada | 100% |
| Duplicatas | Duplicatas visíveis no radar | 0 após normalização |
| Explicabilidade | Resultados com justificativa de compatibilidade | 100% |
| Aplicações confirmadas | Contagem de registros explicitamente marcados como enviados | Sem falsos positivos |
| Utilidade ao usuário zero | Feedback qualitativo após cada sessão | Melhorar iterativamente |

A **contratação** é objetivo final do usuário, mas não pode ser prometida como resultado do produto.

## 8. O que não seremos

- Agregador que replica conteúdo sem autorização.
- Robô que dispara centenas de candidaturas ou burla CAPTCHA.
- Sistema que coleta laudos e diagnósticos indiscriminadamente.
- IA que inventa experiências, cursos, níveis de inglês ou competências.
- Marketplace de dados pessoais de candidatos.

## 9. Modelo de evolução

- **0.1 / usuário zero:** produto local, perfil mestre, quatro currículos, radar real com integração permitida e importação manual, matching simples, acompanhamento.
- **0.2 / beta fechado:** múltiplos usuários, autenticação, consentimentos e exclusão/exportação de dados, mais fontes, alertas e testes de usabilidade.
- **0.3 / produto:** personalização avançada de currículo, explicação semântica assistida por IA quando útil, indicadores e preparação para entrevistas.
- **Posterior:** parcerias e monetização testadas somente após valor demonstrado, direitos de integração e proteção de dados revisados.

## 10. Riscos principais

| Risco | Mitigação |
|---|---|
| Fontes não oferecem API de busca geral | Registro de empresas/fontes, APIs oficiais por empresa, importação manual de links |
| API muda, limita ou restringe uso | Adaptadores isolados, política de acesso por fonte, logs, backoff e desativação por configuração |
| Vagas antigas ou falsas | Origem, timestamps separados, empresa obrigatória, estado de verificação e revisão humana |
| Perfil incorreto | Edição e confirmação explícita de dados; nunca gerar fatos inexistentes |
| Exposição de dados pessoais/sensíveis | Repositório sem PII, armazenamento local protegido e consentimento específico para qualquer dado sensível |
| Escopo excessivo | Uma jornada vertical funcional antes de adicionar IA, extensões e automações |

## 11. Decisões vigentes

- Nome de trabalho: **VagaFlow**.
- Prioridade nº 1.
- **Primeiro caso de uso vale mais que arquitetura sofisticada.**
- Execução inicial local (sem hospedagem obrigatória).
- Dados fictícios em testes/repositório; dados reais apenas em armazenamento local privado.
- Cadência: pequenos incrementos, testes, revisão e evidências de execução.

---
