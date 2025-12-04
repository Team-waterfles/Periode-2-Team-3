using Microsoft.Maui.Controls;

namespace BasisJaar2.Views
{
    public partial class LevelsPage : ContentView
    {
        public LevelsPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.LevelsViewModel();
        }
    }
}
