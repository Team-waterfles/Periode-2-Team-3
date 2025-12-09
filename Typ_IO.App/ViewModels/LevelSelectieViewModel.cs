using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BasisJaar2.Models;
using BasisJaar2.Views;

namespace BasisJaar2.ViewModels
{
    public class LevelSelectieViewModel : BindableObject
    {
        public ObservableCollection<Level> Levels { get; } = new();
        public ICommand StartLevelCommand { get; }

        private readonly string _difficulty;

        public LevelSelectieViewModel(string difficulty)
        {
            _difficulty = difficulty.ToLower();
            LoadLevels();
            StartLevelCommand = new Command<Level>(OnStartLevel);
        }

        private void LoadLevels()
        {
            Levels.Clear();

            switch (_difficulty)
            {
                case "makkelijk":
                    Levels.Add(new Level { Nummer = "level1", Naam = "Basis Letters", Beschrijving = "A, S, D, F, J, K, L" });
                    Levels.Add(new Level { Nummer = "level2", Naam = "Uitgebreid", Beschrijving = "Meer letters en combinaties" });
                    break;

                case "gemiddeld":
                    Levels.Add(new Level { Nummer = "level3", Naam = "Woorden", Beschrijving = "Typen van korte woorden" });
                    Levels.Add(new Level { Nummer = "level4", Naam = "Langere woorden", Beschrijving = "Complexere combinaties" });
                    break;

                case "moeilijk":
                    Levels.Add(new Level { Nummer = "level5", Naam = "Zinnen", Beschrijving = "Volledige zinnen oefenen" });
                    break;
            }
        }

        private void OnStartLevel(Level level)
        {
            if (level == null) return;

            // Sla het geselecteerde level op
            PracticeSession.GeselecteerdLevel = level;

            // Navigeer naar oefening
            if (MainPageViewModel.Current != null)
            {
                var currentPage = MainPageViewModel.Current.SubpageContent;
                MainPageViewModel.Current.SubpageContent =
                    new Oefening(level.Nummer.ToString(), "play", currentPage);
            }
        }
    }
}
