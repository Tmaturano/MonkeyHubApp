using MonkeyHubApp.Services;
using MonkeyHubApp.ViewModels;
using Xamarin.Forms;

namespace MonkeyHubApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var monkeyHubApiService = DependencyService.Get<IMonkeyHubApiService>();
            BindingContext = new MainViewModel(monkeyHubApiService);
        }

        protected override async void OnAppearing()
        {
            await (BindingContext as MainViewModel)?.LoadAsync();

            base.OnAppearing();            
        }
    }
}
