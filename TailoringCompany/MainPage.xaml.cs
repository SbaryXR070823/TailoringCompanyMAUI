namespace TailoringCompany
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Sucked his dick {count} time";
            else
                CounterBtn.Text = $"Sucked his dick {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
