using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BasisJaar2.Models;
using BasisJaar2.Views;
using Typ_IO.Core.Repositories;
using Typ_IO.Core.Models;

namespace BasisJaar2.ViewModels
{
    public class LevelSelectieViewModel : BindableObject
    {
        public ObservableCollection<Standaardlevel> Levels { get; } = new();
        public ICommand StartLevelCommand { get; }

        private readonly int _difficulty;
        private readonly IStandaardlevelRepository _levelrepository;
        public LevelSelectieViewModel(int difficulty)
        {
            _difficulty = difficulty;
            _levelrepository = Application.Current.Windows[0].Page.Handler.MauiContext.Services.GetService<IStandaardlevelRepository>();
            LoadLevels();
            StartLevelCommand = new Command<Standaardlevel>(OnStartLevel);
        }

        private async void LoadLevels()
        {
            Levels.Clear();

            foreach (var level in await _levelrepository.ListByDifficultyAsync(_difficulty))
            {
                Levels.Add(level);
            }
        }

        private void OnStartLevel(Standaardlevel level)
        {
            if (level == null) return;

            // Sla het geselecteerde level op
            PracticeSession.GeselecteerdLevel = new Level {Id = 0, Naam = level.Naam, Beschrijving = "Geen beschrijving", Tekst = level.Tekst };

            // Navigeer naar oefening
            if (MainPageViewModel.Current != null)
            {
                var currentPage = MainPageViewModel.Current.SubpageContent;
                MainPageViewModel.Current.SubpageContent =
                    new Oefening(PracticeSession.GeselecteerdLevel, "play", currentPage);
            }
        }
    }
}
