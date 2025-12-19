using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BasisJaar2.Models;
using BasisJaar2.Views;

namespace BasisJaar2.ViewModels;

public class LevelsViewModel : BindableObject
{
    public ObservableCollection<Level> Levels { get; } = new();
    public ICommand StartLevelCommand { get; }

    public LevelsViewModel()
    {
        Levels.Add(new Level { Nummer = "1", Naam = "Level 1 - 2 vingers" });
        Levels.Add(new Level { Nummer = "2", Naam = "Level 2 - 4 vingers" });
        Levels.Add(new Level { Nummer = "3", Naam = "Level 3 - 6 vingers" });
        Levels.Add(new Level { Nummer = "4", Naam = "Level 4 - 8 vingers" });
        Levels.Add(new Level { Nummer = "5", Naam = "Level 5 - letters bovenste rij" });
        Levels.Add(new Level { Nummer = "6", Naam = "Level 6 - alle vingers" });

        StartLevelCommand = new Command<Level>(OnStartLevel);
    }

    private void OnStartLevel(Level level)
    {
        if (level == null) return;

        if (!PracticeSession.IsLevelUnlocked(int.Parse(level.Nummer)))
        {
            Application.Current.MainPage.DisplayAlert(
                "Level vergrendeld",
                $"Je moet eerst level {int.Parse(level.Nummer) - 1} halen.",
                "OK");
            return;
        }

        // Zet geselecteerd level
        PracticeSession.GeselecteerdLevel = level;

        if (MainPageViewModel.Current != null)
        {
            var currentPage = MainPageViewModel.Current.SubpageContent;
            MainPageViewModel.Current.SubpageContent =
                new Oefening(level.Nummer.ToString(), null, currentPage);
        }
    }
}
