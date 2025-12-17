using BasisJaar2.ViewModels;
using BasisJaar2.Models;
using System.Linq;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace BasisJaar2.Views;

public partial class SongMode : ContentView
{
    private readonly SongModeViewModel _viewModel;

    public SongMode()
    {
        InitializeComponent();
        _viewModel = new SongModeViewModel();
        BindingContext = _viewModel;

        Debug.WriteLine($"SongMode initialized. Songs count: {_viewModel.Songs.Count}");

        // Zet placeholder tekst
        TitelLabel.Text = "Selecteer een song";
        ArtiestLabel.Text = "";
        MoeilijkheidLabel.Text = "";
        BpmLabel.Text = "";
        LengteLabel.Text = "";
    }

    private void OnSongTapped(object sender, TappedEventArgs e)
    {
        Debug.WriteLine($"Song tapped! Parameter: {e.Parameter}");

        if (e.Parameter is string songTitel)
        {
            Debug.WriteLine($"Looking for song: {songTitel}");

            var song = _viewModel.Songs.FirstOrDefault(s => s.Titel == songTitel);

            if (song != null)
            {
                Debug.WriteLine($"Found song: {song.Titel}, BPM: {song.Bpm}");
                _viewModel.SelecteerSongCommand.Execute(song);

                // Update de labels direct
                TitelLabel.Text = song.Titel;
                ArtiestLabel.Text = song.Artiest;
                MoeilijkheidLabel.Text = song.Moeilijkheid;
                BpmLabel.Text = $"BPM {song.Bpm}";
                LengteLabel.Text = $"Length {song.LengteFormatted}";

                Debug.WriteLine($"Labels updated! BPM: {BpmLabel.Text}");
            }
            else
            {
                Debug.WriteLine("Song NOT found!");
            }
        }
    }

    private void OnPlayClicked(object sender, EventArgs e)
    {
        _viewModel.PlaySongCommand.Execute(null);
    }
}