using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BasisJaar2.Models;

namespace BasisJaar2.ViewModels
{
    public class SongModeViewModel : BindableObject
    {
        private Song _geselecteerdeSong;

        public ObservableCollection<Song> Songs { get; } = new();

        public Song GeselecteerdeSong
        {
            get => _geselecteerdeSong;
            set
            {
                _geselecteerdeSong = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSongGeselecteerd));
            }
        }

        public bool IsSongGeselecteerd => GeselecteerdeSong != null;

        public ICommand SelecteerSongCommand { get; }
        public ICommand PlaySongCommand { get; }
        public ICommand TerugCommand { get; }

        public SongModeViewModel()
        {
            SelecteerSongCommand = new Command<Song>(OnSelecteerSong);
            PlaySongCommand = new Command(OnPlaySong);
            TerugCommand = new Command(OnTerug);

            LaadSongs();
        }

        private void LaadSongs()
        {
            Songs.Add(new Song
            {
                Titel = "Enemy",
                Artiest = "Imagine Dragons",
                Moeilijkheid = "Medium",
                Bpm = 120,
                Lengte = new System.TimeSpan(0, 3, 34)
            });

            Songs.Add(new Song
            {
                Titel = "Believer",
                Artiest = "Imagine Dragons",
                Moeilijkheid = "Easy",
                Bpm = 125,
                Lengte = new System.TimeSpan(0, 3, 24)
            });

            Songs.Add(new Song
            {
                Titel = "Faded",
                Artiest = "Alan Walker",
                Moeilijkheid = "Easy",
                Bpm = 90,
                Lengte = new System.TimeSpan(0, 3, 32)
            });

            Songs.Add(new Song
            {
                Titel = "Bad Romance",
                Artiest = "Lady Gaga",
                Moeilijkheid = "Hard",
                Bpm = 140,
                Lengte = new System.TimeSpan(0, 4, 54)
            });
        }

        private void OnSelecteerSong(Song song)
        {
            if (song == null) return;
            GeselecteerdeSong = song;
        }

        private void OnPlaySong()
        {
            if (GeselecteerdeSong == null)
            {
                Application.Current.MainPage.DisplayAlert(
                    "Geen song geselecteerd",
                    "Selecteer eerst een song om te spelen.",
                    "OK");
                return;
            }

            // Hier kan je later de navigatie naar het speel-scherm toevoegen
            Application.Current.MainPage.DisplayAlert(
                "Song starten",
                $"Nu spelen: {GeselecteerdeSong.Titel} - {GeselecteerdeSong.Artiest}",
                "OK");
        }

        private void OnTerug()
        {
            if (MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent = new Views.StartScreen();
            }
        }
    }
}