import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

interface PageData {
  heading: string;
  description: string;
  planned: string;
}

@Component({
  selector: 'vf-placeholder',
  standalone: true,
  imports: [RouterLink],
  template: `
    <main id="conteudo" class="content placeholder" aria-labelledby="page-title">
      <span class="eyebrow">Em construção · MVP 0.1-alpha</span>
      <h1 id="page-title">{{ page.heading }}</h1>
      <p class="placeholder-description">{{ page.description }}</p>
      <section class="coming-soon" aria-labelledby="planned-title">
        <h2 id="planned-title">Próxima entrega</h2>
        <p>{{ page.planned }}</p>
        <p>Esta tela é apenas a estrutura inicial. Ainda não existe cadastro, busca ou envio real.</p>
      </section>
      <a routerLink="/" class="btn btn-outline">← Voltar ao início</a>
    </main>
  `
})
export class PlaceholderPageComponent {
  private readonly route = inject(ActivatedRoute);
  protected readonly page = this.route.snapshot.data as PageData;
}
