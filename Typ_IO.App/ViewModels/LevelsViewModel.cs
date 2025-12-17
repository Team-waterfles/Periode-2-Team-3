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
        AddLevel("1", "Level 1 - 2 vingers", "Basis met de twee wijsvingers (F en J).");
        AddLevel("2", "Level 2 - 4 vingers", "Meer vingers, focus op ritme en nauwkeurigheid.");
        AddLevel("3", "Level 3 - 6 vingers", "Nog meer vingers, afwisseling links en rechts.");
        AddLevel("4", "Level 4 - 8 vingers", "Alle vingers op de thuisrij (asdf jkl;).");
        AddLevel("5", "Level 5 - bovenste rij", "Letters van de bovenste rij oefenen (qwer tyui op).");
        AddLevel("6", "Level 6 - alle vingers", "Combinatie van bovenste rij, thuisrij en onderste rij.");
        AddLevel("7", "Level 7 - korte woorden", "Korte, eenvoudige woorden achter elkaar typen.");
        AddLevel("8", "Level 8 - zinnen zonder leestekens", "Rustige zinnen zonder hoofdletters of leestekens.");
        AddLevel("9", "Level 9 - zinnen met leestekens", "Hoofdletters, komma’s en punten oefenen.");
        AddLevel("10", "Level 10 - lange tekst", "Doorlopende tekst, alsof je een artikel overtikt.");

        StartLevelCommand = new Command<Level>(OnStartLevel);
    }

    private void AddLevel(string nummer, string naam, string beschrijving)
    {
        int levelNummer = int.Parse(nummer);

        var level = new Level
        {
            Nummer = nummer,
            Naam = naam,
            Beschrijving = beschrijving,
            IsUnlocked = PracticeSession.IsLevelUnlocked(levelNummer),
            IsCompleted = PracticeSession.IsLevelCompleted(levelNummer)
        };

        Levels.Add(level);
    }

    public void RefreshLevelStates()
    {
        foreach (var level in Levels)
        {
            int num = int.Parse(level.Nummer);
            level.IsUnlocked = PracticeSession.IsLevelUnlocked(num);
            level.IsCompleted = PracticeSession.IsLevelCompleted(num);
        }
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

        PracticeSession.GeselecteerdLevel = level;

        if (MainPageViewModel.Current != null)
        {
            var currentPage = MainPageViewModel.Current.SubpageContent;
            MainPageViewModel.Current.SubpageContent =
                new Oefening(level.Nummer.ToString(), "play", currentPage);
        }
    }
}
