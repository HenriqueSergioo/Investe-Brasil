import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ServiceBase } from '../../../shared/service-base';
import { InvestimentoDetalheEntity } from '../entities/investimento-detalhe.entity';
import { InvestimentoListagemEntity } from '../entities/investimento-listagem.entity';
import { PaginacaoEntity } from '../entities/paginacao.entity';
import { IncluirInvestimentoRequestEntity } from '../entities/requests/incluir-investimento.request.entity';
import { ResgatarInvestimentoRequestEntity } from '../entities/requests/resgatar-investimento.request.entity';

@Injectable({
  providedIn: 'root'
})
export class InvestimentoService extends ServiceBase {
  constructor(private http: HttpClient) {
    super();
  }

  consultarInvestimentos(
    nomeProprietario: string,
    dataInicial: string,
    dataFinal: string,
    pagina: number,
    tamanhoPagina: number
  ): Observable<PaginacaoEntity<InvestimentoListagemEntity>> {
    return this.http.get<PaginacaoEntity<InvestimentoListagemEntity>>(
      `${environment.apiInvestimentos}Investimento/ConsultarInvestimentos`,
      {
        params: this.httpParams({
          nomeProprietario,
          dataInicial,
          dataFinal,
          pagina,
          tamanhoPagina
        })
      }
    );
  }

  consultarInvestimento(id: number, dataReferencia?: string): Observable<InvestimentoDetalheEntity> {
    return this.http.get<InvestimentoDetalheEntity>(
      `${environment.apiInvestimentos}Investimento/ConsultarInvestimento/${id}`,
      { params: this.httpParams({ dataReferencia }) }
    );
  }

  consultarSaldoGeral(): Observable<{ saldo: number }> {
    return this.http.get<{ saldo: number }>(`${environment.apiInvestimentos}Investimento/ConsultarSaldoGeral`);
  }

  incluirInvestimento(request: IncluirInvestimentoRequestEntity): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(
      `${environment.apiInvestimentos}Investimento/IncluirInvestimento`,
      request
    );
  }

  resgatarInvestimento(request: ResgatarInvestimentoRequestEntity): Observable<InvestimentoDetalheEntity> {
    return this.http.post<InvestimentoDetalheEntity>(
      `${environment.apiInvestimentos}Investimento/ResgatarInvestimento`,
      request
    );
  }
}
