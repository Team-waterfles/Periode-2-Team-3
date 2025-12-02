using BasisJaar2.ViewModels;

namespace BasisJaar2.Views
{
    public partial class MoeilijkheidsgraadPage : ContentView
    {
        private string _gekozenMoeilijkheidsgraad = "Makkelijk"; // Default waarde

        public MoeilijkheidsgraadPage()
        {
            InitializeComponent();
            var viewModel = new MoeilijkheidsgraadViewModel();
            viewModel.NavigeerNaarLevelSelectie += OnNavigeerNaarLevelSelectie;
            viewModel.MoeilijkheidsgraadGekozen += OnMoeilijkheidsgraadGekozen;
            BindingContext = viewModel;
        }

        private void OnMoeilijkheidsgraadGekozen(object? sender, string moeilijkheidsgraad)
        {
            _gekozenMoeilijkheidsgraad = moeilijkheidsgraad;
        }

        private void OnNavigeerNaarLevelSelectie(object? sender, EventArgs e)
        {
            var levelSelectie = new LevelSelectie();
            levelSelectie.LevelGeselecteerd += OnLevelGeselecteerd;
            levelSelectie.TerugNavigatie += OnTerugVanLevelSelectie;

            // Gebruik MainPageViewModel om de content te wijzigen
            if (MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent = levelSelectie;
            }
        }

        private void OnLevelGeselecteerd(object? sender, int levelNummer)
        {
            // Selecteer de juiste oefening pagina op basis van level en moeilijkheidsgraad
            ContentView? oefening = null;

            // Voor nu alleen Makkelijk implementeren
            if (_gekozenMoeilijkheidsgraad == "Makkelijk")
            {
                switch (levelNummer)
                {
                    case 1:
                        oefening = new OefeningLevel1();
                        break;
                    case 2:
                        oefening = new OefeningLevel2();
                        break;
                    case 3:
                        oefening = new OefeningLevel3();
                        break;
                    case 4:
                        oefening = new OefeningLevel4();
                        break;
                    case 5:
                        oefening = new OefeningLevel5();
                        break;
                    default:
                        oefening = new Oefening(); // Fallback naar standaard oefening
                        break;
                }
            }
            else if (_gekozenMoeilijkheidsgraad == "Normaal")
            {
                oefening = new Oefening(); // Tijdelijk fallback
            }
            else if (_gekozenMoeilijkheidsgraad == "Moeilijk")
            {
                oefening = new Oefening(); // Tijdelijk fallback
            }

            if (oefening != null && MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent = oefening;
            }
        }

        private void OnTerugVanLevelSelectie(object? sender, EventArgs e)
        {
            // Terug naar moeilijkheidsgraad pagina
            if (MainPageViewModel.Current != null)
            {
                MainPageViewModel.Current.SubpageContent = this;
            }
        }
    }
}