using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Futuristic.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
            OpenShareCommand = new Command(async () => await Share.RequestAsync(new ShareTextRequest
            {
                Uri = "https://play.google.com/store/apps/details?id=com.futuristic.sface",
                Title = "SFACE - Easily find store lineups"
            }));
        }

        public ICommand OpenWebCommand { get; }
        public ICommand OpenShareCommand { get; }
    }
}