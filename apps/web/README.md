# VagaFlow Web — VF-003

Shell de aplicação **Angular 21 LTS + TypeScript**, com layout responsivo, rotas acessíveis e páginas de estado vazio.

**Importante:** o Radar agora consulta a API local VF-010 e permite cadastrar anúncios manualmente quando API + PostgreSQL estiverem disponíveis. A plataforma ainda **não pesquisa vagas na Internet por conta própria** e não envia candidaturas.

## Executar localmente (CachyOS/Linux)

Pré-requisito: Node.js **22.12+**, ou Node 20.19+ com suporte para Angular 21.

```bash
cd apps/web
npm install
npm start
```

Abra `http://127.0.0.1:4200`.

### Qualidade

```bash
npm test
npm run build
```

O primeiro `npm install` produz `package-lock.json`; **versione-o após uma instalação validada** e troque para `npm ci` no CI, permitindo builds reprodutíveis. O ambiente inicial da automação de criação não permite baixar dependências e, por isso, a compilação precisa ser verificada no GitHub Actions ou na sua máquina.

## Rotas

| Caminho | Tela |
|---|---|
| `/` | Visão geral |
| `/perfil` | Perfil mestre (previsto VF-005/006) |
| `/curriculos` | Currículos (previsto VF-007) |
| `/radar` | Cadastro manual e Radar local de vagas (VF-010/013) |
| `/candidaturas` | Acompanhamento (previsto VF-017/018) |

Não há chamadas externas de API de vagas nem armazenamento de dados pessoais nesta etapa.

## Radar em desenvolvimento (VF-013)

Inicie PostgreSQL e API conforme os respectivos READMEs; ambos devem estar acessíveis em `127.0.0.1` (API na porta 5050). Na aplicação Angular, abra `http://127.0.0.1:4200/radar`. A API permite a origem local `http://127.0.0.1:4200` somente em ambiente Development.

- Cadastro rápido: empresa, título, URL HTTPS, descrição bruta, modalidade, cidade/UF, área; data de publicação somente se confirmada no anúncio.
- Lista inicial vem exclusivamente do banco local; **nenhuma vaga fictícia** é apresentada.
- Filtros: empresa/cargo, área, Teresina/PI, remoto (elegibilidade para o Brasil deve ser conferida), publicação confirmada desde 01/11/2026 ou data desconhecida.
- Abrir o link original **não equivale a se candidatar**.
- Ainda não há login ou autenticação: nunca exponha a API para a Internet.
