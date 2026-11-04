import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ControlsModule } from '../../controls/controls.module';
import { CriarInvestimentoComponent } from './criar/criar-investimento.component';
import { DetalheInvestimentoComponent } from './detalhe/detalhe-investimento.component';
import { InvestimentoComponent } from './investimento.component';
import { InvestimentoRoutingModule } from './investimento-routing.module';
import { ListarInvestimentoComponent } from './listar/listar-investimento.component';
import { ResgatarListaComponent } from './resgatar-lista/resgatar-lista.component';
import { ResgatarInvestimentoComponent } from './resgatar/resgatar-investimento.component';

@NgModule({
  declarations: [
    ListarInvestimentoComponent,
    CriarInvestimentoComponent,
    DetalheInvestimentoComponent,
    ResgatarListaComponent,
    ResgatarInvestimentoComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ControlsModule,
    InvestimentoComponent,
    InvestimentoRoutingModule
  ]
})
export class InvestimentoModule {}
