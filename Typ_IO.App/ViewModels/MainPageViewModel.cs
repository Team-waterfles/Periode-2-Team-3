using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace BasisJaar2.ViewModels
{
    public class MainPageViewModel : BindableObject
    {
        // Globale referentie zodat andere viewmodels kunnen navigeren
        public static MainPageViewModel? Current { get; private set; }

        private ContentView? _subpageContent;
        public ContentView? SubpageContent
        {
            get => _subpageContent;
            set
            {
                _subpageContent = value;
                OnPropertyChanged();
                UpdateToolbarProperties();
            }
        }

        // Hulpproperty: zijn we in de Settings-pagina?
        private bool IsInSettings => SubpageContent is Views.Settings;

        private string? _toolbarIcon;
        public string? ToolbarIcon
        {
            get => _toolbarIcon;
            set { _toolbarIcon = value; OnPropertyChanged(); }
        }

        private string? _toolbarText;
        public string? ToolbarText
        {
            get => _toolbarText;
            set { _toolbarText = value; OnPropertyChanged(); }
        }

        public ICommand ToolbarCommand { get; }

        public MainPageViewModel()
        {
            // static instance zetten
            Current = this;

            // Startscherm als eerste pagina
            SubpageContent = new Views.StartScreen();

            ToolbarCommand = new Command(OnToolbarClicked);
            UpdateToolbarProperties();
        }

        private void OnToolbarClicked()
        {
            if (IsInSettings)
                SubpageContent = new Views.StartScreen();
            else
                SubpageContent = new Views.Settings();
        }

        private void UpdateToolbarProperties()
        {
            if (IsInSettings)
            {
                // In settings: toon "Exit" (terug-knop)
                ToolbarIcon = null;   // geen icoon
                ToolbarText = "Exit"; // tekstknop
            }
            else
            {
                // Normaal: toon settings-icoon zonder tekst
                ToolbarIcon = "settings.png";
                ToolbarText = null;
            }
        }
    }
}