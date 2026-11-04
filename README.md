# InvesteBrasil

Aplicação fullstack para criação, consulta e resgate de investimentos, com ganho composto mensal e tributação sobre o rendimento.

O projeto foi organizado no mesmo recorte da Feev: backend em C# com camadas Controller → Service → Validador → Repository → Query, e frontend em Angular com módulos, rotas lazy, services e entities.

## Como executar

### Backend

Pré-requisito: .NET 8.

```bash
cd backend
dotnet test
dotnet run --project InvesteBrasil.Investimento.Api
```

A API sobe em [http://localhost:5000](http://localhost:5000).  
Documentação Swagger: [http://localhost:5000/swagger](http://localhost:5000/swagger)

A API exige o Neon. Coloque `DATABASE_URL` em `backend/.env` (arquivo local, fora do Git). Na subida, as queries criam as tabelas `usuario` e `investimento` no Postgres. Cada investimento fica ligado ao usuário autenticado (`COD_USUARIO`); a lista, o saldo e o resgate usam só os registros daquela conta. As colunas seguem o padrão Feev (`COD_`, `NOM_`, `DAT_`, `VLR_`, `DES_`, `STA_`).

### Frontend

Pré-requisito: Node.js 18+.

```bash
cd frontend
npm install
npm start
```

A interface sobe em [http://localhost:4200](http://localhost:4200).

O cadastro de usuário é feito na tela de login e grava na tabela `usuario`.

Backend e frontend sobem de forma independente. Documentação da API: [http://localhost:5000/swagger](http://localhost:5000/swagger).

## Regras de negócio

- Rendimento de 0,52% ao mês, no mesmo dia da criação, em juros compostos.
- Resgate sempre total.
- Imposto apenas sobre o ganho:
  - menos de 1 ano: 22,5%
  - de 1 a 2 anos: 18,5%
  - 2 anos ou mais: 15%
- Investimento resgatado aparece com saldo atual zerado na listagem. O detalhe conserva ganhos, imposto e valor líquido da data do resgate.

## Como testar os cálculos

O rendimento **não é diário**. Os 0,52% só entram no aniversário mensal (mesmo dia da aplicação). Um investimento criado hoje, com retirada no mesmo mês, mostra ganho e imposto **R$ 0,00** — isso está certo.

Para conferir ganho, saldo esperado e imposto na hora, crie o investimento com **data da aplicação no passado** (hoje ou anterior; a API não aceita data futura). Exemplos com **R$ 1.000,00**:

| Data da aplicação | O que conferir | Alíquota |
| --- | --- | --- |
| Hoje | Ganho R$ 0,00, saldo = valor investido | 22,5% de zero |
| 1 mês atrás (mesmo dia) | 1 rendimento: saldo **R$ 1.005,20** | 22,5% |
| Menos de 1 ano (ex.: 11 meses) | Juros compostos mês a mês | **22,5%** sobre o ganho |
| Exatamente 1 ano atrás | Completou 1 ano | **18,5%** sobre o ganho |
| 2 anos ou mais | Alíquota mínima | **15%** sobre o ganho |

No resgate, a **data do resgate** também pode ser qualquer dia entre a aplicação e hoje. Mude essa data para simular outro prazo e ver a alíquota / o valor líquido antes de confirmar.

Os testes automatizados (`dotnet test` no backend) cobrem o juros composto, o arredondamento mês a mês e as três faixas de imposto.

## Bibliotecas

### Backend

- ASP.NET Core 8: host HTTP e controllers
- Swashbuckle: documentação da API
- Npgsql: acesso ao Postgres/Neon via ADO.NET, no estilo das queries da Feev
- xUnit: testes do cálculo de ganho, imposto e validações

### Frontend

- Angular 17 + TypeScript: UI, rotas e módulos
- RxJS: consumo HTTP
- FormsModule: formulários template-driven, no padrão Feev

## Estrutura

```
backend/
  InvesteBrasil.Infra/
  InvesteBrasil.Investimento/
  InvesteBrasil.Investimento.Api/
  InvesteBrasil.Investimento.Tests/
  Database/Scripts/
frontend/
  src/app/modules/investimento/
  src/app/shared/
  src/app/_layout/
screenshots/
```
