using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using BasisJaar2.ViewModels;

namespace BasisJaar2.Views
{
    public partial class PracticePage : ContentView
    {
        public PracticePage()
        {
            InitializeComponent();
        }

        private void OnUserTextChanged(object sender, TextChangedEventArgs e)
        {
            if (LiveFeedbackLabel == null)
                return;

            if (BindingContext is not PracticePageViewModel vm)
                return;

            string referentie = vm.Oefentekst ?? string.Empty;
            string input = e.NewTextValue ?? string.Empty;

            var formatted = new FormattedString();

            int maxLen = input.Length;
            for (int i = 0; i < maxLen; i++)
            {
                char inpChar = input[i];
                char refChar = i < referentie.Length ? referentie[i] : '\0';

                var span = new Span { Text = inpChar.ToString() };

                if (i < referentie.Length && inpChar == refChar)
                    span.TextColor = Colors.Green;
                else
                    span.TextColor = Colors.Red;

                formatted.Spans.Add(span);
            }

            LiveFeedbackLabel.FormattedText = formatted;
        }
    }
}