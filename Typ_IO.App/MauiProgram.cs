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
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // DI-registraties
            builder.Services.AddSingleton<IMySqlConnectionFactory>(_ => new MySqlConnectionFactory());
            // builder.Services.AddSingleton<MySqlSchemaMigrator>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Schema migreren + seeden
            // using var scope = app.Services.CreateScope();
            // var migrator = scope.ServiceProvider.GetRequiredService<MySqlSchemaMigrator>();

            // var task = migrator.MigrateAsync();
            // task.GetAwaiter().GetResult();
            //task = SeedAsync(scope.ServiceProvider);
            //task.GetAwaiter().GetResult();

            ServiceHelper.Initialize(app.Services);

            return app;
        }
    }
}
