import test from 'node:test';
import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';

const read = (relativePath) => readFileSync(new URL(relativePath, import.meta.url), 'utf8');

test('rota Radar usa componente funcional e HttpClient é registrado', () => {
  const routes = read('../src/app/app.routes.ts');
  const main = read('../src/main.ts');
  assert.match(routes, /component: RadarComponent/);
  assert.match(main, /provideHttpClient\(\)/);
});

test('registro manual exige empresa, cargo e URL original HTTPS', () => {
  const form = read('../src/app/radar.component.html');
  for (const field of ['name="company"', 'name="title"', 'name="url"']) {
    assert.ok(form.includes(field), field);
  }
  assert.match(form, /pattern="https:\/\/.+"/);
  assert.match(form, /publicationDateVerified/);
  assert.match(form, /Salvar vaga para revisão/);
});

test('a interface não marca abertura do portal como candidatura enviada', () => {
  const form = read('../src/app/radar.component.html');
  const logic = read('../src/app/radar.component.ts');
  assert.match(form, /Abrir o anúncio/);
  assert.match(form, /target="_blank"/);
  assert.match(form, /rel="noopener noreferrer"/);
  assert.match(logic, /publishedAtUtc: this\.draft\.publicationDateVerified/);
  assert.match(logic, /\/manual/);
  assert.doesNotMatch(logic, /localStorage\./);
});
