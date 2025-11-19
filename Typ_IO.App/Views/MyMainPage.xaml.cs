namespace BasisJaar2.Views;

public partial class MyMainPage : ContentPage
{
	public MyMainPage()
	{
		InitializeComponent();
    }
    void OnPage1Clicked(object sender, EventArgs e)
    {
        Subpage.Content = new Page1();
    }
    void OnPage2Clicked(object sender, EventArgs e)
    {
        Subpage.Content = new Page2();
    }
}