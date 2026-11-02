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

Sem connection string, a API usa um repositório em memória (com dados de exemplo).  
Quando o Neon estiver disponível, preencha `ConnectionStrings:Neon` em `backend/InvesteBrasil.Investimento.Api/appsettings.json`. O script da tabela está em `backend/Database/Scripts/001_criar_tabela_investimento.sql` e também é aplicado automaticamente na subida.

### Frontend

Pré-requisito: Node.js 18+.

```bash
cd frontend
npm install
npm start
```

A interface sobe em [http://localhost:4200](http://localhost:4200).

Usuários de demonstração:

- `henrique` / `123456`
- `admin` / `admin123`

Backend e frontend sobem de forma independente.

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
