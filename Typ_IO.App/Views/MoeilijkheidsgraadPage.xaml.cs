using BasisJaar2.ViewModels;

namespace BasisJaar2.Views
{
    public partial class MoeilijkheidsgraadPage : ContentView
    {
        public MoeilijkheidsgraadPage()
        {
            InitializeComponent();
            BindingContext = new MoeilijkheidsgraadViewModel();
        }
    }
}