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
    public ObservableCollection<Oefenlevel> Levels { get; } = new();
    public ICommand StartLevelCommand { get; }
    private readonly IOefenlevelRepository _levelrepository;

    public LevelsViewModel()
    {
        _levelrepository = Application.Current.Windows[0].Page.Handler.MauiContext.Services.GetService<IOefenlevelRepository>();
        GetLevels();

        StartLevelCommand = new Command<Oefenlevel>(OnStartLevel);
    }

    private async void GetLevels()
    {
        foreach (var level in await _levelrepository.ListAsync())
        {
            Levels.Add(level);
        }

    }

    private void OnStartLevel(Oefenlevel level)
    {
        if (level == null) return;

        if (!PracticeSession.IsLevelUnlocked(level.Id))
        {
            Application.Current.MainPage.DisplayAlert(
                "Level vergrendeld",
                $"Je moet eerst level het vorige level halen.",
                "OK");
            return;
        }

        // Zet geselecteerd level
        string tekst = Levelgenerator.MaakLevelBijLetteropties(level.Letteropties, 100);
        PracticeSession.GeselecteerdLevel = new Level { Id = level.Id, Naam = level.Naam, Tekst = tekst, Beschrijving = "Geen beschrijving" };

        if (MainPageViewModel.Current != null)
        {
            var currentPage = MainPageViewModel.Current.SubpageContent;
            MainPageViewModel.Current.SubpageContent =
                new Oefening(PracticeSession.GeselecteerdLevel, true, currentPage);
        }
    }
}
