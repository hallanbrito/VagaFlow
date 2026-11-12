# ADR-0003 — Entrega antecipada, importação rápida e currículos por afinidade

**Estado:** aceito para planejamento, sujeito à validação no primeiro uso  
**Data:** 2026-11-12  
**Contexto:** revisão crítica do plano mestre durante a fundação de VagaFlow.

## Decisões
1. **Dois marcos:** `0.1-alpha` entrega o radar manual operável antes do gerador ATS e da integração Greenhouse; `0.1` conclui a experiência planejada.
2. **Cobertura regional:** colar URL e texto bruto de uma vaga local é o primeiro caminho de entrada. A extração sugere campos, mas o usuário revisa empresa, título, região, modalidade e data antes de salvar. Não buscar URLs arbitrárias automaticamente.
3. **Áreas distintas:** classificamos TI, LOGISTICA, ADMINISTRACAO e ATENDIMENTO sem tratar as duas últimas como Logística. A versão Logística do currículo pode ser sugerida como **fallback revisável**, apenas quando experiência transferível sustentar a recomendação. Caso contrário, sugerir revisão manual. Evolução futura: perfil específico de Administração/Atendimento.
4. **PcD:** a variante PcD depende de autorização explícita e do contexto do processo seletivo, sem extrapolar informações clínicas.
5. **Geração de PDF:** preferir uma prova de conceito .NET com QuestPDF; verificar texto extraível, links e leitura ATS antes de substituir o renderizador. QuestPDF não é software sob licença OSI. A Community License é gratuita para usos elegíveis (incluindo comerciais): https://www.questpdf.com/license/community.html.
6. **Sequência:** VF-008 e VF-012 continuam planejadas para a versão completa 0.1, mas não bloqueiam o marco 0.1-alpha.

## Consequências
O usuário zero poderá usar a primeira entrega para procurar vagas reais em Teresina e remoto Brasil sem depender da cobertura de um ATS estrangeiro. O risco de um fallback automático inadequado é mitigado pela revisão humana e por evidência apresentada.
