using Microsoft.Extensions.Logging;

namespace BasisJaar2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
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
            var migrator = scope.ServiceProvider.GetRequiredService<MySqlSchemaMigrator>();

            var task = migrator.MigrateAsync();
            task.GetAwaiter().GetResult();
            task = SeedAsync(scope.ServiceProvider);
            task.GetAwaiter().GetResult();

            ServiceHelper.Initialize(app.Services);

            return app;
        }
    }
}
