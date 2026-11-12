# ADR-0001 — MVP local-first com monólito modular

**Estado:** aceito para o MVP 0.1  
**Data:** 2026-11-12

## Contexto
O usuário zero precisa encontrar vagas úteis rapidamente, com orçamento inicial próximo de zero. Microsserviços, filas distribuídas e serviços pagos aumentariam o custo e a complexidade antes da validação.

## Decisão
- Front-end Angular + TypeScript, API ASP.NET Core/.NET 10 e PostgreSQL local (Docker Compose).
- Separar módulos lógicos de Perfil, Currículos, Fontes/Vagas, Matching e Candidaturas.
- Contratos e testes por módulo; dependências externas encapsuladas em adaptadores.
- Fonte de vagas inicial: importação manual + ao menos um provedor de leitura permitido.
- Permitir usar o radar mesmo antes do gerador avançado de currículo estar pronto.

## Consequências
Menos esforço operacional e entrega incremental. A separação modular deixa aberta uma futura extração de workers caso volume justifique. A escala multiusuário está fora do MVP.
