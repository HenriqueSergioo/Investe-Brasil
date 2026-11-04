import { Routes } from '@angular/router';

export const Routing: Routes = [
  {
    path: 'investimentos',
    loadChildren: () =>
      import('../modules/investimento/investimento.module').then(m => m.InvestimentoModule)
  },
  { path: '', pathMatch: 'full', redirectTo: 'investimentos' }
];
