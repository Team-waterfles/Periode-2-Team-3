using System.Reflection.PortableExecutable;
using BasisJaar2.Helpers;
using BasisJaar2.ViewModels;
using BasisJaar2.Views;
using Microsoft.Extensions.Logging;
using Typ_IO.Core.Data;
using Typ_IO.Core.Models;
using Typ_IO.Core.Repositories;
using Windows.UI.Xaml.Interop;

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
            Standaardlevel[] standaardlevels = [
                new Standaardlevel {Moeilijkheidsgraad = 1, Naam = "Testlevel1", Tekst = "Pa's wijze lynx bezag vroom het fikse aquaduct" },
                new Standaardlevel {Moeilijkheidsgraad = 1, Naam = "Testlevel2", Tekst = "Typ deze zin zorgvuldig om je snelheid te testen" },
                new Standaardlevel {Moeilijkheidsgraad = 1, Naam = "Testlevel3", Tekst = "Dit is een moeilijkere oefening voor gevorderde typers" },
                new Standaardlevel {Moeilijkheidsgraad = 2, Naam = "Woorden", Tekst = "Hallo wereld auto fiets computer toetsenbord oefen typen leren school student programmeren muziek ritme spelen piano gitaar drums zingen Nederland Amsterdam Rotterdam Utrecht Groningen" },
                new Standaardlevel {Moeilijkheidsgraad = 2, Naam = "Complexere combinaties", Tekst = "De kat zit op de mat. De hond rent door het park. Typen leren is leuk en nuttig voor iedereen. Muziek en ritme helpen bij het oefenen. Practice makes perfect, dus blijf oefenen!" },
                new Standaardlevel {Moeilijkheidsgraad = 3, Naam = "Volledige zinnen", Tekst = "Dit is een uitgebreide typoefening voor gevorderde gebruikers. Typen is een belangrijke vaardigheid in de moderne wereld. Of je nu student bent, professional of hobbyist, goede typvaardigheden maken je werk een stuk efficiënter. Door regelmatig te oefenen met verschillende soorten teksten, verbeter je niet alleen je snelheid maar ook je nauwkeurigheid. Muziek en ritme kunnen helpen om je typritme te verbeteren en het oefenen aangenamer te maken." }];
            await SeedStandaardlevelAsync(sp.GetRequiredService<IStandaardlevelRepository>(), standaardlevels);

            Oefenlevel[] oefenlevels =[
             new Oefenlevel { Id = 0, Naam = "2 vingers", Letteropties = " fj" },
             new Oefenlevel { Id = 1, Naam = "4 vingers", Letteropties = " fjdk" },
             new Oefenlevel { Id = 2, Naam = "6 vingers", Letteropties = " fjdkwslo" },
             new Oefenlevel { Id = 3, Naam = "8 vingers", Letteropties = " asdfjkl;" },
             new Oefenlevel { Id = 4, Naam = "bovenste rij", Letteropties = " qwertyuiop" },
             new Oefenlevel { Id = 5, Naam = "onderste rij", Letteropties = " zxcvbnm" },
             new Oefenlevel { Id = 6, Naam = "letters (alles)", Letteropties = " qwertyuiopasdfghjklzxcvbnm" },
             new Oefenlevel { Id = 7, Naam = "letters + ;", Letteropties = " ;qwertyuiopasdfghjklzxcvbnm" },
             new Oefenlevel { Id = 8, Naam = "woorden (mix)", Letteropties = " qwertyuiopasdfghjklzxcvbnm" },
             new Oefenlevel { Id = 9, Naam = "moeilijke mix", Letteropties = " ;qwertyuiopasdfghjklzxcvbnm" },
];

            await SeedOefenlevelAsync(sp.GetRequiredService<IOefenlevelRepository>(), oefenlevels);

        }
        private static async Task SeedStandaardlevelAsync(IStandaardlevelRepository levelrepository, Standaardlevel[] levels)
        {
            List<Standaardlevel> levels_in_database = levelrepository.ListAsync().Result;
            foreach (Standaardlevel level in levels)
            {
                Standaardlevel? orginele_level = levels_in_database.Find(l => l.Naam == level.Naam);
                if (orginele_level == null)
                {
                    await levelrepository.AddAsync(level);
                }
                // Als er al een level bestaat met dezelfde naam moet deze bijgewerkt worden
                else
                {
                    orginele_level.Tekst = level.Tekst;
                    orginele_level.Moeilijkheidsgraad = level.Moeilijkheidsgraad;
                    await levelrepository.UpdateAsync(orginele_level);
                }
            }
        }
        private static async Task SeedOefenlevelAsync(IOefenlevelRepository levelrepository, Oefenlevel[] levels)
        {
            foreach (Oefenlevel level in levels)
            {
                if (levelrepository.GetByIdAsync(level.Id).Result == null)
                {
                    await levelrepository.AddAsync(level);
                }
                else
                {
                    await levelrepository.UpdateAsync(level);
                }
            }
        }
    }
}
