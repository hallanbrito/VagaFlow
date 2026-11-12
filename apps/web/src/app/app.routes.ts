import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { RadarComponent } from './radar.component';
import { PlaceholderPageComponent } from './placeholder-page.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', component: DashboardComponent, title: 'Início | VagaFlow' },
  {
    path: 'perfil',
    component: PlaceholderPageComponent,
    title: 'Meu perfil | VagaFlow',
    data: {
      heading: 'Meu perfil profissional',
      description: 'Aqui você organizará experiências, formação, competências e preferências de vagas em um perfil mestre.',
      planned: 'Cadastro de perfil e preferências — VF-005 e VF-006.'
    }
  },
  {
    path: 'curriculos',
    component: PlaceholderPageComponent,
    title: 'Meus currículos | VagaFlow',
    data: {
      heading: 'Meus currículos',
      description: 'Você poderá associar versões Gerais e PcD às áreas de atuação, sempre revisando as informações antes de compartilhar.',
      planned: 'Cadastro e importação de PDFs — VF-007.'
    }
  },
  {
    path: 'radar',
    component: RadarComponent,
    title: 'Radar de vagas | VagaFlow'
  },
  {
    path: 'candidaturas',
    component: PlaceholderPageComponent,
    title: 'Candidaturas | VagaFlow',
    data: {
      heading: 'Minhas candidaturas',
      description: 'Um painel para acompanhar candidaturas enviadas por você, evitando duplicações e distinguindo vagas salvas de inscrições efetivamente realizadas.',
      planned: 'Histórico manual de candidaturas — VF-017 e VF-018.'
    }
  },
  { path: '**', redirectTo: '' }
];
