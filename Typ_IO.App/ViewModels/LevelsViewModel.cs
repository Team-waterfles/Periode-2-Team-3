using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BasisJaar2.Models;
using BasisJaar2.Views;
using Typ_IO.Core.Repositories;
using Typ_IO.Core.Models;
using Typ_IO.Core.Services;

namespace BasisJaar2.ViewModels;

public class LevelsViewModel : BindableObject
{
    public ObservableCollection<Level> Levels { get; } = new();
    public ICommand StartLevelCommand { get; }

    private readonly IOefenlevelRepository _repo;

    // DB-levels in volgorde
    private List<Oefenlevel> _dbLevels = new();

    public LevelsViewModel()
    {
        _repo = Application.Current.Windows[0].Page.Handler.MauiContext.Services.GetService<IOefenlevelRepository>();
        StartLevelCommand = new Command<Level>(OnStartLevel);
        LoadLevels();
    }

    private async void LoadLevels()
    {
        Levels.Clear();

        // ✅ vaste volgorde (zelfde volgorde als jouw seed: op Naam werkt meestal, maar wil je seed-volgorde exact? zeg het)
        _dbLevels = (await _repo.ListAsync())
            .OrderBy(x => x.Naam)
            .ToList();

        int count = Math.Min(10, _dbLevels.Count);

        for (int i = 0; i < count; i++)
        {
            int progressId = i + 1; // ✅ 1..10
            var db = _dbLevels[i];

            Levels.Add(new Level
            {
                Id = progressId,
                Nummer = progressId.ToString(),
                Naam = db.Naam,

                // ✅ hier komt jouw vinger/letters uitleg terug
                Beschrijving = GetBeschrijving(progressId, db.Letteropties),

                Tekst = "",
                IsUnlocked = PracticeSession.IsLevelUnlocked(progressId),
                IsCompleted = PracticeSession.IsLevelCompleted(progressId)
            });
        }
    }

    public void RefreshLevelStates()
    {
        foreach (var level in Levels)
        {
            level.IsUnlocked = PracticeSession.IsLevelUnlocked(level.Id);
            level.IsCompleted = PracticeSession.IsLevelCompleted(level.Id);
        }
    }

    private void OnStartLevel(Level level)
    {
        if (level == null) return;

        int progressId = level.Id; // 1..10

        // ✅ HARD BLOCK
        if (!PracticeSession.IsLevelUnlocked(progressId))
        {
            Application.Current.MainPage.DisplayAlert(
                "Level vergrendeld",
                "Je moet eerst het vorige level halen.",
                "OK");
            return;
        }

        int index = progressId - 1;
        if (index < 0 || index >= _dbLevels.Count)
        {
            Application.Current.MainPage.DisplayAlert("Fout", "Level bestaat niet in database.", "OK");
            return;
        }

        var dbLevel = _dbLevels[index];

        // ✅ tekst voor oefening (random op basis van letteropties)
        string tekst = Levelgenerator.MaakLevelBijLetteropties(dbLevel.Letteropties, 100);

        PracticeSession.GeselecteerdLevel = new Level
        {
            Id = progressId,
            Nummer = level.Nummer,
            Naam = level.Naam,
            Beschrijving = level.Beschrijving,
            Tekst = tekst,
            IsUnlocked = true,
            IsCompleted = PracticeSession.IsLevelCompleted(progressId)
        };

        if (MainPageViewModel.Current != null)
        {
            var currentPage = MainPageViewModel.Current.SubpageContent;

            // ✅ PLAY (geen practice hints)
            MainPageViewModel.Current.SubpageContent =
                new Oefening(PracticeSession.GeselecteerdLevel, true, currentPage);
        }
    }

    private static string GetBeschrijving(int progressId, string letteropties)
    {
        // korte "letters:" string voor UI
        string letters = string.IsNullOrWhiteSpace(letteropties)
            ? ""
            : letteropties.Trim();

        return progressId switch
        {
            1 => $"Letters: {letters} • Wijsvingers (F/J). Focus: ritme + basispositie.",
            2 => $"Letters: {letters} • Wijs- en middelvingers (F/J/D/K). Focus: afwisseling links/rechts.",
            3 => $"Letters: {letters} • Meer spreiding. Focus: tempo vasthouden zonder fouten.",
            4 => $"Letters: {letters} • Thuisrij (ASDF JKL;). Focus: alle vingers op home-row.",
            5 => $"Letters: {letters} • Bovenste rij (QWERTYUIOP). Focus: bewegen vanaf thuisrij.",
            6 => $"Letters: {letters} • Onderste rij (ZXCVBNM). Focus: controle met ring/pink + duimen (spatie).",
            7 => $"Letters: {letters} • Hele alfabet. Focus: snelheid + consistentie.",
            8 => $"Letters: {letters} • Alfabet + ;. Focus: moeilijke aanslagen rechterpink (;).",
            9 => $"Letters: {letters} • Mix oefenen. Focus: langere reeksen zonder te stoppen.",
            10 => $"Letters: {letters} • Moeilijke mix. Focus: nauwkeurigheid ≥ 90% en steady WPM.",
            _ => $"Letters: {letters} • Oefen met de juiste vingers."
        };
    }
}