using BasisJaar2.Views;
using Microsoft.Maui.Controls;

namespace BasisJaar2.Views;

public partial class MyMainPage : ContentPage
{
    public MyMainPage()
    {
        InitializeComponent();
        BindingContext = new ViewModels.MainPageViewModel();
    }
}
