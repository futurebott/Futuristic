using Futuristic.Models;
using Futuristic.Services;
using Futuristic.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace Futuristic.ViewModels
{
    public class StoreListViewModel : BaseViewModel
    {
        public ObservableCollection<Store> stores { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command SelectionChanged { get; set; }
        private StoreService _storeService;

        public StoreListViewModel()
        {
            stores = new ObservableCollection<Store>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            Device.StartTimer(new TimeSpan(0, 5, 0), () =>
            {
                // do something every 60 seconds
                Device.BeginInvokeOnMainThread(() =>
                {
                    Task.Run(async () => await ExecuteLoadItemsCommand());
                });
                return true; // runs again, or false to stop
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                if (stores.Any())
                    stores.Clear();
                _storeService = new StoreService();
                var userLocation = await UserService.Instance.CurrentLocation();
                var parameters = "latitude=" + userLocation.Latitude + "&longtitude=" + userLocation.Longitude + "&live=true";
                var asnycList = await _storeService.GetList(parameters);
                foreach (var item in asnycList.OrderBy(a => a.Distance))
                {
                    if (item.Distance > 0)
                        item.DistanceString = LocationMonanager.MetersToString(item.Distance);
                    item.OpenCloseTime = Utilities.OpenCloseTime(item.OpenTime, item.CloseTime, Utilities.GetNowDateTime().Hour, out string timeLabel);
                    item.TimeLabel = timeLabel;
                    if (timeLabel == "Close")
                        item.TimeLabelColor = "Red";
                    else
                        item.TimeLabelColor = "Green";
                    if (item.OpenCloseTime == "Closed")
                    {
                        item.CheckOutLine = 0;
                        item.OutSideLine = 0;
                    }
                    stores.Add(item);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }



    }
}
