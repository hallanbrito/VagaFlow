import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { firstValueFrom } from 'rxjs';

type JobArea = 'it' | 'logistics' | 'administration' | 'customerService' | 'other';
type WorkArrangement = 'unknown' | 'remote' | 'hybrid' | 'onSite';

interface Job {
  id: string;
  company: string;
  title: string;
  originalUrl: string;
  sourcePortal: string;
  description: string;
  area: JobArea;
  workArrangement: WorkArrangement;
  city: string | null;
  state: string | null;
  publishedAtUtc: string | null;
  firstSeenAtUtc: string;
  verificationState: 'pendingReview' | 'verified' | 'unavailable';
}

interface JobDraft {
  company: string;
  title: string;
  url: string;
  sourcePortal: string;
  description: string;
  area: JobArea;
  workArrangement: WorkArrangement;
  city: string;
  state: string;
  publicationDate: string;
  publicationDateVerified: boolean;
}

const emptyDraft = (): JobDraft => ({
  company: '',
  title: '',
  url: '',
  sourcePortal: '',
  description: '',
  area: 'other',
  workArrangement: 'unknown',
  city: '',
  state: '',
  publicationDate: '',
  publicationDateVerified: false
});

@Component({
  selector: 'vf-radar',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './radar.component.html',
  styleUrl: './radar.component.css'
})
export class RadarComponent implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly apiBase = 'http://127.0.0.1:5050/api/v1/jobs';

  readonly jobs = signal<Job[]>([]);
  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  readonly search = signal('');
  readonly areaFilter = signal('');
  readonly regionFilter = signal('all');
  readonly dateFilter = signal('all');

  readonly areaLabels: Record<JobArea, string> = {
    it: 'Tecnologia',
    logistics: 'Logística',
    administration: 'Administração',
    customerService: 'Atendimento / Call Center',
    other: 'Outra área'
  };

  readonly workLabels: Record<WorkArrangement, string> = {
    unknown: 'Modalidade não informada',
    remote: 'Remoto — confirmar elegibilidade Brasil',
    hybrid: 'Híbrido',
    onSite: 'Presencial'
  };

  readonly filteredJobs = computed(() => {
    const search = this.search().trim().toLocaleLowerCase('pt-BR');
    const area = this.areaFilter();
    const region = this.regionFilter();
    const date = this.dateFilter();

    return this.jobs().filter(job => {
      if (search && !(job.company + ' ' + job.title).toLocaleLowerCase('pt-BR').includes(search)) {
        return false;
      }
      if (area && job.area !== area) {
        return false;
      }
      if (region === 'teresina' &&
          !(job.city?.toLocaleLowerCase('pt-BR') === 'teresina' && job.state === 'PI')) {
        return false;
      }
      if (region === 'remote' && job.workArrangement !== 'remote') {
        return false;
      }
      // No publication date: NEVER assume that collection date is publication date.
      if (date === 'november' &&
          (!job.publishedAtUtc ||
           Date.parse(job.publishedAtUtc) < Date.parse('2026-11-01T00:00:00Z'))) {
        return false;
      }
      if (date === 'unknown' && job.publishedAtUtc) {
        return false;
      }
      return true;
    });
  });

  draft: JobDraft = emptyDraft();

  ngOnInit(): void {
    void this.loadJobs();
  }

  async loadJobs(): Promise<void> {
    this.loading.set(true);
    this.errorMessage.set('');
    try {
      const records = await firstValueFrom(this.http.get<Job[]>(this.apiBase));
      this.jobs.set(records);
    } catch (error: unknown) {
      this.errorMessage.set(this.describeHttpError(error));
    } finally {
      this.loading.set(false);
    }
  }

  async save(): Promise<void> {
    if (this.saving()) return;

    this.saving.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    const request = {
      company: this.draft.company.trim(),
      title: this.draft.title.trim(),
      url: this.draft.url.trim(),
      sourcePortal: this.draft.sourcePortal.trim(),
      description: this.draft.description.trim(),
      area: this.draft.area,
      workArrangement: this.draft.workArrangement,
      city: this.draft.city.trim() || null,
      state: this.draft.state.trim().toUpperCase() || null,
      publishedAtUtc: this.draft.publicationDateVerified && this.draft.publicationDate
        ? this.draft.publicationDate + 'T12:00:00Z'
        : null,
      publicationDateVerified: this.draft.publicationDateVerified
    };

    try {
      const created = await firstValueFrom(this.http.post<Job>(this.apiBase + '/manual', request));
      this.jobs.update(jobs => [created, ...jobs]);
      this.draft = emptyDraft();
      this.successMessage.set('Vaga salva no Radar! Confira o anúncio original antes de se candidatar.');
    } catch (error: unknown) {
      this.errorMessage.set(this.describeHttpError(error));
    } finally {
      this.saving.set(false);
    }
  }

  formatDate(date: string | null): string {
    if (!date) return 'Data de publicação não verificada';
    return new Intl.DateTimeFormat('pt-BR', { dateStyle: 'short', timeZone: 'UTC' })
      .format(new Date(date));
  }

  private describeHttpError(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      if (error.status === 409) return 'Esta vaga já foi registrada. Evitamos duplicações no Radar.';
      if (error.status === 400) return 'Revise os dados: a empresa, o cargo e um link HTTPS externo são necessários.';
      if (error.status === 0) return 'API local indisponível. Inicie a API e o PostgreSQL conforme os READMEs.';
    }
    return 'Não foi possível carregar ou salvar as vagas. Verifique a API e tente novamente.';
  }
}
