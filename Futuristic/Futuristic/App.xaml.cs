using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Futuristic.Services;
using Futuristic.Views;
using System.Threading.Tasks;

namespace Futuristic
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
           // MainPage = new MainPage();
        }

        protected override async void OnStart()
        {
            MainPage = new NavigationPage(new SplashPage());
            try
            {
                string appId = UserService.Instance.GetApplicationId().ToString();
            }
            catch(Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Not Online", "You are not online. try when you are...", "Cancel", "ok");
            }
            await Task.Delay(5000);

            MainPage = new MainPage();
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
