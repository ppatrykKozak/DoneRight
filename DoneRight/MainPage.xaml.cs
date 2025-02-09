using DoneRight.Pages;
namespace DoneRight

{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnTaskClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TasksPage());
        }
    }

}
