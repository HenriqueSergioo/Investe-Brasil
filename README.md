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

O cadastro de usuário é feito na tela de login e grava na tabela `usuario`, inclusive o e-mail para notificação.

Para enviar e-mail de verdade na criação e no resgate, preencha as variáveis `SMTP_*` no `backend/.env`. Sem SMTP, a notificação só aparece no log da API.

Backend e frontend sobem de forma independente. Documentação da API: [http://localhost:5000/swagger](http://localhost:5000/swagger).

## Regras de negócio

- Rendimento de 0,52% ao mês, no mesmo dia da criação, em juros compostos.
- Resgate sempre total.
- Imposto apenas sobre o ganho:
  - menos de 1 ano: 22,5%
  - de 1 a 2 anos: 18,5%
  - 2 anos ou mais: 15%
- Investimento resgatado aparece com saldo atual zerado na listagem. O detalhe conserva ganhos, imposto e valor líquido da data do resgate.

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

Não há envio real de e-mail. Existe `INotificacaoService` com implementação em log, pronta para um provedor SMTP depois.

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
