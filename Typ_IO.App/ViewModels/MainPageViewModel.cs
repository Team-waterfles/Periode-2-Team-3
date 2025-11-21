using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace BasisJaar2.ViewModels
{
    public class MainPageViewModel : BindableObject
    {
        // dit is nieuw! Een static property zodat we overal bij kunnen
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

        private bool _isInSettings => SubpageContent is Views.Settings;

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
            // zet de Current property zodat andere classes er bij kunnen
            Current = this;

            SubpageContent = new Views.StartScreen();
            ToolbarCommand = new Command(OnToolbarClicked);
            UpdateToolbarProperties();
        }

        private void OnToolbarClicked()
        {
            if (_isInSettings)
                SubpageContent = new Views.StartScreen();
            else
                SubpageContent = new Views.Settings();
        }

        private void UpdateToolbarProperties()
        {
            if (_isInSettings)
            {
                ToolbarIcon = null;           // Important for text-only button
                ToolbarText = "Exit";          // Must be non-empty
            }
            else
            {
                ToolbarIcon = "settings.png";  // Icon for default state
                ToolbarText = null;            // No text
            }
        }
    }
}