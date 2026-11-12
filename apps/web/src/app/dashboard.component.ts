import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'vf-dashboard',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent {}
