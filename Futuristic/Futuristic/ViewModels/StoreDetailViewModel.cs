using Futuristic.Models;
using Futuristic.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Futuristic.ViewModels
{

    public class StoreDetailViewModel: BaseViewModel
    {
        public Store SingleStore { get; set; }
        public Command BtnCmdDirection { get; private set; }
        private LineUpService _lineUpService;
        public StoreDetailViewModel(Store store = null)
        {
            Title = store?.Name;
            SingleStore = store;
            //SingleStore.WeeklyTime = Utilities.WeekTimes(SingleStore.commaStoreTimings);
            //OnPropertyChanged("WeeklyTime");
            _lineUpService = new LineUpService();
            //BtnCmdLine = new Command<string>(async (param) => await ExecuteButtonClick(param));
            BtnCmdDirection = new Command(async () => await GetDirections());

        }
        private int _OutSideLine;
        private int _SliderValue;
        private int _weeklyTime;

        public string WeeklyTime
        {
          
            get
            {
                return Utilities.WeekTimes(SingleStore.commaStoreTimings);
            }
        }
        public int OutSideLine
        {
            set
            {
                if (value > 0)
                {
                   
                    Task.Run(async () => {

                        if(await GetIdle(value))
                           await ExecuteButtonClick("out-" + _OutSideLine);
                    });
                    _OutSideLine = value;
                    _SliderValue = value;
                    OnPropertyChanged();
                   
                }
            }
            get
            {
                if (_OutSideLine > 0)
                    return _OutSideLine;
                else
                    return 1;

            }
        }
        private int _CheckOutLine;
        public int CheckOutLine
        {
            set
            {
                if (value > 0)
                {

                    Task.Run(async () => {
                        if (await GetIdle(value))
                            await ExecuteButtonClick("in-" + _CheckOutLine);
                    });
                    _CheckOutLine = value;
                    _SliderValue = value;
                    OnPropertyChanged();

                }
            }
            get
            {
                if (_CheckOutLine > 0)
                    return _CheckOutLine;
                else
                    return 1;
            }
        }
        async Task GetDirections()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                // https://developer.apple.com/library/ios/featuredarticles/iPhoneURLScheme_Reference/MapLinks/MapLinks.html
                await Launcher.OpenAsync("http://maps.apple.com/?daddr="+SingleStore.PostalCode);
            }
            else //(Device.RuntimePlatform == Device.Android)
            {
                // opens the 'task chooser' so the user can pick Maps, Chrome or other mapping app
                await Launcher.OpenAsync("http://maps.google.com/?daddr="+SingleStore.PostalCode);
            }
           
        }
        public async Task<bool> GetIdle(int value)
        {
            int sliderValue = value;
            await Task.Delay(500);
            return sliderValue == _SliderValue && !IsBusy;
        }
        async Task ExecuteButtonClick(string value)
        {
            IsBusy = true;
           var btnValueArr = value.Split('-');
            if (btnValueArr.Length > 1)
            {
                var user  = UserService.Instance;
                string lineType = btnValueArr[0];
                int lineCount = int.Parse(btnValueArr[1]);
                var userLocation = user.CurrentLocation().Result;
                var lineObj = new LineUp()
                {
                    ApplicationId = user.GetApplicationId(),
                    LineCount = lineCount,
                    LineType = lineType,
                    StoreId = SingleStore.Id,
                    Time = DateTime.UtcNow,
                    LocationLatitude = userLocation.Latitude,
                    LocationLongtitude = userLocation.Longitude,
                };
               
               await _lineUpService.AddUpdateEntity(lineObj); ;
               IsBusy = false;
            }
        }
    }
}
