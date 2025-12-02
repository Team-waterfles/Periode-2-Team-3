using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BasisJaar2.Models;

namespace BasisJaar2.ViewModels
{
    public class LevelsViewModel : BindableObject
    {
        public ObservableCollection<Level> Levels { get; } = new();
        public ICommand StartLevelCommand { get; }

        public LevelsViewModel()
        {
            // LEVEL 1 – 2 vingers (index)
            Levels.Add(new Level
            {
                Nummer = 1,
                Naam = "Level 1 - 2 vingers",
                Beschrijving = "Alleen indexvingers op F en J (basispositie).",
                Oefentekst = "fj fj fj fj jf jf jf jf"
            });

            // LEVEL 2 – 4 vingers (index + middelvingers)
            Levels.Add(new Level
            {
                Nummer = 2,
                Naam = "Level 2 - 4 vingers",
                Beschrijving = "Index + middelvingers op D K E I.",
                Oefentekst = "fjdj kfj djek fjek idid kejf"
            });

            // LEVEL 3 – 6 vingers (index + middel + ring)
            Levels.Add(new Level
            {
                Nummer = 3,
                Naam = "Level 3 - 6 vingers",
                Beschrijving = "Ringvingers erbij op S en L + W en O.",
                Oefentekst = "wslo slwo fdsj slwo wofs dslj"
            });

            // LEVEL 4 – 8 vingers (volledige home row)
            Levels.Add(new Level
            {
                Nummer = 4,
                Naam = "Level 4 - 8 vingers",
                Beschrijving = "Alle vingers op de home row: ASDF JKL;.",
                Oefentekst = "asdf jkl; asdf jkl; sadf lask fjda ;lkj"
            });

            // LEVEL 5 – 8 vingers + bovenste rij (letters)
            Levels.Add(new Level
            {
                Nummer = 5,
                Naam = "Level 5 - letters bovenste rij",
                Beschrijving = "Home row + bovenste rij: QWERTYUIOP.",
                Oefentekst = "qwer tyui opqw type type quit quit write type"
            });

            // LEVEL 6 – 10 vingers (alle letters)
            Levels.Add(new Level
            {
                Nummer = 6,
                Naam = "Level 6 - alle vingers",
                Beschrijving = "Alle tien vingers, volledige letter-toetsen.",
                Oefentekst = "asdf jkl; qwer tyui zxcv bnm type snel met alle vingers"
            });

            StartLevelCommand = new Command<Level>(OnStartLevel);
        }

        private void OnStartLevel(Level level)
        {
            if (level == null)
                return;

            // gekozen level bewaren voor de oefenpagina
            PracticeSession.GeselecteerdLevel = level;

            if (MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent =
                    new BasisJaar2.Views.PracticePage();
            }
            else
            {
                Application.Current?.MainPage?.DisplayAlert(
                    "Error",
                    "MainPageViewModel not found",
                    "OK");
            }
        }
    }
}
