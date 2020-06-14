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
            Init();
            DependencyService.Register<MockDataStore>();
           // MainPage = new MainPage();
        }

        protected override async void OnStart()
        {
            MainPage = new NavigationPage(new SplashPage());
            await Task.Delay(5000);
            MainPage = new MainPage();
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        protected void Init()
        {
            try
            {
               UserService.Instance.Init();
            }
            catch (Exception ex)
            {
                Task.Run(async() => { 
                    await Application.Current.MainPage.DisplayAlert("Not Online", "You are not online. try when you are...", "Cancel", "ok");
                });
            }
        }
    }
}
