using BasisJaar2.Helpers;
using Typ_IO.Core.Data;
using Typ_IO.Core.Models;
using Typ_IO.Core.Repositories;
using BasisJaar2.Views;
using BasisJaar2.ViewModels;
using Microsoft.Extensions.Logging;

namespace BasisJaar2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            // DB-pad (Lokale database)
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // DI-registraties

            builder.Services.AddSingleton<ISqliteConnectionFactory>(_ => new SqliteConnectionFactory(dbPath));
            builder.Services.AddSingleton<SqliteSchemaMigrator>();

            builder.Services.AddScoped<IStandaardlevelRepository, StandaardlevelRepository>();
            builder.Services.AddScoped<IOefenlevelRepository, OefenlevelRepository>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Schema migreren + seeden
            using var scope = app.Services.CreateScope();
            var migrator = scope.ServiceProvider.GetRequiredService<SqliteSchemaMigrator>();

            var task = migrator.MigrateAsync();
            task.GetAwaiter().GetResult();

            // Data die in de database komt als een gebruiker de applicatie voor het eerst opstart
            task = SeedAsync(scope.ServiceProvider);
            task.GetAwaiter().GetResult();


            ServiceHelper.Initialize(app.Services);

            return app;
        }
        private static async Task SeedAsync(IServiceProvider sp)
        {
            var standaardlevels = sp.GetRequiredService<IStandaardlevelRepository>();
            var oefenlevels = sp.GetRequiredService<IOefenlevelRepository>();

            if ((await standaardlevels.ListAsync()).Count == 0)
            {
                var level = new Standaardlevel
                {
                    Naam = "Oefenlevel",
                    Tekst = "Pa's wijze lynx bezag vroom het fikse aquaduct",
                    Moeilijkheidsgraad = 1
                };
                await standaardlevels.AddAsync(level);
            }

            if ((await oefenlevels.ListAsync()).Count == 0)
            {
                var level = new Oefenlevel
                {
                    Naam = "2 vingers",
                    Letteropties = "fj "
                };
                await oefenlevels.AddAsync(level);
            }
        }
    }
}
