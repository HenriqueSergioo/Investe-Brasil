using InvesteBrasil.Investimento.Api;

CarregarArquivoEnv(Path.Combine(Directory.GetCurrentDirectory(), ".env"));
CarregarArquivoEnv(Path.Combine(Directory.GetCurrentDirectory(), "..", ".env"));

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app);
app.Run();

static void CarregarArquivoEnv(string caminho)
{
    if (!File.Exists(caminho))
        return;

    foreach (var linha in File.ReadAllLines(caminho))
    {
        var texto = linha.Trim();
        if (texto.Length == 0 || texto.StartsWith('#') || !texto.Contains('='))
            continue;

        var indice = texto.IndexOf('=');
        var chave = texto[..indice].Trim();
        var valor = texto[(indice + 1)..].Trim().Trim('"').Trim('\'');
        if (chave.Length == 0 || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(chave)))
            continue;

        Environment.SetEnvironmentVariable(chave, valor);
    }
}
