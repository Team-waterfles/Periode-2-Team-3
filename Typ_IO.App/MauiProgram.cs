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
            var standaardlevelrepository = sp.GetRequiredService<IStandaardlevelRepository>();
            var oefenlevelrepository = sp.GetRequiredService<IOefenlevelRepository>();

            if ((await standaardlevelrepository.ListAsync()).Count == 0)
            {
                Standaardlevel[] standaardlevels = [
                    new Standaardlevel {Moeilijkheidsgraad = 1, Naam = "Testlevel1", Tekst = "Pa's wijze lynx bezag vroom het fikse aquaduct" },
                    new Standaardlevel {Moeilijkheidsgraad = 1, Naam = "Testlevel2", Tekst = "Typ deze zin zorgvuldig om je snelheid te testen" },
                    new Standaardlevel {Moeilijkheidsgraad = 1, Naam = "Testlevel3", Tekst = "Dit is een moeilijkere oefening voor gevorderde typers" },
                    new Standaardlevel {Moeilijkheidsgraad = 1, Naam = "Woordenlijst", Tekst = "Hallo wereld auto fiets computer toetsenbord oefen typen leren school student programmeren muziek ritme spelen piano gitaar drums zingen Nederland Amsterdam Rotterdam Utrecht Groningen" },
                    new Standaardlevel {Moeilijkheidsgraad = 2, Naam = "Simpele tekst", Tekst = "De kat zit op de mat. De hond rent door het park. Typen leren is leuk en nuttig voor iedereen. Muziek en ritme helpen bij het oefenen. Practice makes perfect, dus blijf oefenen!" },
                    new Standaardlevel {Moeilijkheidsgraad = 3, Naam = "Geavanceerde tekst", Tekst = "Dit is een uitgebreide typoefening voor gevorderde gebruikers. Typen is een belangrijke vaardigheid in de moderne wereld. Of je nu student bent, professional of hobbyist, goede typvaardigheden maken je werk een stuk efficiënter. Door regelmatig te oefenen met verschillende soorten teksten, verbeter je niet alleen je snelheid maar ook je nauwkeurigheid. Muziek en ritme kunnen helpen om je typritme te verbeteren en het oefenen aangenamer te maken." }];
                
                foreach (Standaardlevel level in standaardlevels)
                {
                    await standaardlevelrepository.AddAsync(level);
                }
                
            }

            if ((await oefenlevelrepository.ListAsync()).Count == 0)
            {
                Oefenlevel[] oefenlevels = [
                    new Oefenlevel {Naam = "2 vingers", Letteropties = " fj" },
                    new Oefenlevel {Naam = "4 vingers", Letteropties = " fjdke" },
                    new Oefenlevel {Naam = "6 vingers", Letteropties = " fjdkewslo" },
                    new Oefenlevel {Naam = "8 vingers", Letteropties = " ;fjdkewsloa" },
                    new Oefenlevel {Naam = "letters bovenste rij", Letteropties = " qwertyuiop" },
                    new Oefenlevel {Naam = "10 vingers", Letteropties = " ;qwertyuiopasdfghjklzxcvbnm" }];

                foreach (Oefenlevel level in oefenlevels)
                {
                    await oefenlevelrepository.AddAsync(level);
                }
            }
        }
    }
}
