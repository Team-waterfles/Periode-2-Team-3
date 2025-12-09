namespace BasisJaar2.Views;

public partial class StartScreen : ContentView
{
	public StartScreen()
	{
		InitializeComponent();
        BindingContext = new ViewModels.StartScreenViewModel();
    }
}