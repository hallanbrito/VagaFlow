# VagaFlow Web — VF-003

Shell de aplicação **Angular 21 LTS + TypeScript**, com layout responsivo, rotas acessíveis e páginas de estado vazio.

**Importante:** a interface ainda **não busca vagas**, não armazena dados e não envia candidaturas. São rotas preparadas para as issues seguintes.

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
| `/radar` | Vagas (previsto VF-010/013) |
| `/candidaturas` | Acompanhamento (previsto VF-017/018) |

Não há chamadas externas de API de vagas nem armazenamento de dados pessoais nesta etapa.
