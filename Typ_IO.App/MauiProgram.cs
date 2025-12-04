using BasisJaar2.Helpers;
using Typ_IO.Core.Data;
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

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Schema migreren + seeden
            using var scope = app.Services.CreateScope();
            var migrator = scope.ServiceProvider.GetRequiredService<SqliteSchemaMigrator>();

            var task = migrator.MigrateAsync();
            task.GetAwaiter().GetResult();
            // SeedAsync nog toevoegen.
            // (Dit is voor data die in de database komt als een gebruiker de applicatie voor het eerst opstart)

            ServiceHelper.Initialize(app.Services);

            return app;
        }
    }
}
