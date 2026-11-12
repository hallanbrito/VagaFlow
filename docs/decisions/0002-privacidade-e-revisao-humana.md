# ADR-0002 — Privacidade, informação verificável e revisão humana

**Estado:** aceito para o MVP 0.1  
**Data:** 2026-11-12

## Contexto
Currículos contêm dados pessoais, e alguns candidatos poderão optar por versões PcD. O VagaFlow também precisa evitar vagas falsas, datas inventadas e candidaturas duplicadas.

## Decisão
- Não versionar dados reais, currículos, laudos ou identificadores pessoais no repositório público.
- Dados de teste exclusivamente sintéticos; backend local não exposto publicamente por padrão.
- Perfil editável com fatos confirmados; não inferir experiências, idiomas ou certificações.
- Informações PcD opcionais e sob escolha explícita; o MVP não armazena laudo ou diagnóstico.
- Vaga recomendável exige empresa identificada e URL de origem; publicação comprovada e coleta são campos diferentes.
- Score apresenta explicações e não promete chance de contratação.
- Abrir o portal não significa candidatar-se; somente o usuário confirma status `enviada`.
- Nenhum envio automático ou automação de CAPTCHA no MVP.

## Consequências
Mais confiabilidade e menor risco de exposição; a candidatura permanece uma ação consciente do usuário. Antes de expandir para beta multiusuário, realizar análise formal de LGPD, segurança e controle de acesso.
