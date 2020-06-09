using Futuristic.Models;
using Futuristic.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Futuristic.ViewModels
{

    public class StoreDetailViewModel: BaseViewModel
    {
        public Store SingleStore { get; set; }
        public Command BtnCmdLine { get; private set; }
        private LineUpService _lineUpService;
        public StoreDetailViewModel(Store store = null)
        {
            Title = store?.Name;
            SingleStore = store;
            _lineUpService = new LineUpService();
            BtnCmdLine = new Command<string>(async (param) => await ExecuteButtonClick(param));

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
