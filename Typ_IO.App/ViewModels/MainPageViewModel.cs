using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace BasisJaar2.ViewModels
{
    public class MainPageViewModel : BindableObject
    {
        // static reference zodat andere viewmodels kunnen navigeren
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

        private bool IsInSettings => SubpageContent is Views.Settings;

        public MainPageViewModel()
        {
            // globale referentie voor navigatie
            Current = this;

            // start op het startscherm
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
                ToolbarIcon = null;  // tekstknop "Exit"
                ToolbarText = "Exit";
            }
            else
            {
                ToolbarIcon = "settings.png"; // icoon tonen
                ToolbarText = null;
            }
        }
    }
}
