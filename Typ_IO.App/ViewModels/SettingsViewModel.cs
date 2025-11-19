using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace BasisJaar2.ViewModels
{
    public class SettingsViewModel : BindableObject
    {
        // Colors
        public Color ActiveBackground { get; } = Color.FromArgb("#0078D7");
        public Color InactiveBackground { get; } = Color.FromArgb("#061f4f");
        public Color ActiveText { get; } = Colors.White;
        public Color InactiveText { get; } = Colors.White;

        private string _activeTabContent;
        public string ActiveTabContent
        {
            get => _activeTabContent;
            set { _activeTabContent = value; OnPropertyChanged(); }
        }

        private string _activeTab;
        public string ActiveTab
        {
            get => _activeTab;
            set { _activeTab = value; OnPropertyChanged(); OnPropertyChanged(nameof(MainTabBackground)); OnPropertyChanged(nameof(AudioTabBackground)); OnPropertyChanged(nameof(VideoTabBackground)); }
        }

        // Tab backgrounds
        public Color MainTabBackground => ActiveTab == "Main" ? ActiveBackground : InactiveBackground;
        public Color AudioTabBackground => ActiveTab == "Audio" ? ActiveBackground : InactiveBackground;
        public Color VideoTabBackground => ActiveTab == "Video" ? ActiveBackground : InactiveBackground;

        // Tab text colors
        public Color MainTabTextColor => ActiveTab == "Main" ? ActiveText : InactiveText;
        public Color AudioTabTextColor => ActiveTab == "Audio" ? ActiveText : InactiveText;
        public Color VideoTabTextColor => ActiveTab == "Video" ? ActiveText : InactiveText;

        // Commands
        public ICommand SelectMainTabCommand { get; }
        public ICommand SelectAudioTabCommand { get; }
        public ICommand SelectVideoTabCommand { get; }

        public SettingsViewModel()
        {
            // Default tab
            ActiveTab = "Main";
            ActiveTabContent = "Main settings here";

            SelectMainTabCommand = new Command(() =>
            {
                ActiveTab = "Main";
                ActiveTabContent = "Main settings here";
            });

            SelectAudioTabCommand = new Command(() =>
            {
                ActiveTab = "Audio";
                ActiveTabContent = "Audio settings here";
            });

            SelectVideoTabCommand = new Command(() =>
            {
                ActiveTab = "Video";
                ActiveTabContent = "Video settings here";
            });
        }
    }
}
