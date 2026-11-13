import test from 'node:test';
import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';

const read = (path) => readFileSync(new URL(path, import.meta.url), 'utf8');

test('as quatro rotas principais existem', () => {
  const routes = read('../src/app/app.routes.ts');
  for (const path of ['perfil', 'curriculos', 'radar', 'candidaturas']) {
    assert.match(routes, new RegExp(`path: '${path}'`));
  }
});

test('o shell oferece navegação acessível para teclado', () => {
  const layout = read('../src/app/app.component.html');
  assert.match(layout, /class="skip-link"/);
  assert.match(layout, /aria-label="Navegação principal"/);
  assert.match(layout, /<router-outlet\s*\/>/);
});

test('a interface não anuncia vagas ou envios ainda inexistentes', () => {
  const home = read('../src/app/dashboard.component.html');
  const placeholder = read('../src/app/placeholder-page.component.ts');
  assert.match(home, /funcionalidades ainda não disponíveis/);
  assert.match(placeholder, /Ainda não existe cadastro, busca ou envio real/);
});
