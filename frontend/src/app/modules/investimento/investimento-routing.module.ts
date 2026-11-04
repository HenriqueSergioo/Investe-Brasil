import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CriarInvestimentoComponent } from './criar/criar-investimento.component';
import { DetalheInvestimentoComponent } from './detalhe/detalhe-investimento.component';
import { InvestimentoComponent } from './investimento.component';
import { ListarInvestimentoComponent } from './listar/listar-investimento.component';
import { ResgatarInvestimentoComponent } from './resgatar/resgatar-investimento.component';

const routes: Routes = [
  {
    path: '',
    component: InvestimentoComponent,
    children: [
      { path: '', component: ListarInvestimentoComponent },
      { path: 'novo', component: CriarInvestimentoComponent },
      { path: ':id/resgatar', component: ResgatarInvestimentoComponent },
      { path: ':id', component: DetalheInvestimentoComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvestimentoRoutingModule {}
