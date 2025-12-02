namespace BasisJaar2.Views
{
    public partial class LevelSelectie : ContentView
    {
        public event EventHandler<int>? LevelGeselecteerd;
        public event EventHandler? TerugNavigatie;

        public LevelSelectie()
        {
            InitializeComponent();
        }

        private void OnLevel1Clicked(object sender, EventArgs e)
        {
            LevelGeselecteerd?.Invoke(this, 1);
        }

        private void OnLevel2Clicked(object sender, EventArgs e)
        {
            LevelGeselecteerd?.Invoke(this, 2);
        }

        private void OnLevel3Clicked(object sender, EventArgs e)
        {
            LevelGeselecteerd?.Invoke(this, 3);
        }

        private void OnLevel4Clicked(object sender, EventArgs e)
        {
            LevelGeselecteerd?.Invoke(this, 4);
        }

        private void OnLevel5Clicked(object sender, EventArgs e)
        {
            LevelGeselecteerd?.Invoke(this, 5);
        }

        private void OnTerugClicked(object sender, EventArgs e)
        {
            TerugNavigatie?.Invoke(this, EventArgs.Empty);
        }
    }
}