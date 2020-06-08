using Futuristic.Models;
using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Futuristic.Services
{
    public sealed class UserService : BaseService<User>
    {
       
       private static readonly Lazy<UserService> lazy = new Lazy<UserService>(() => new UserService());

        public static UserService Instance { get { return lazy.Value; } }
        private UserService() : base("Application")
        {
        }
        public  Guid GetApplicationId()
        {
            string infoStream = GetDeviceInfoStream();
            return new Guid("00000000-0000-0000-0000-000000000000");
        }
        public  async Task<Xamarin.Essentials.Location> CurrentLocation()
        {
            var currentLocation = new Xamarin.Essentials.Location();
            try
            {
                var locationManager = new LocationMonanager();
                currentLocation = await locationManager.GetLocationCache();
                return currentLocation;
            }
            catch
            {
                throw;
            }

        }
        public  string GetDeviceInfoStream()
        {
            var DeviceInfo = CrossDeviceInfo.Current;
            var str = DeviceInfo.GenerateAppId();
            // Device Model (SMG-950U, iPhone10,6)
            StringBuilder deviceInforStream = new StringBuilder();
            deviceInforStream.Append("Model:" + DeviceInfo.Model);
            deviceInforStream.Append("Manufacturer:" + DeviceInfo.Manufacturer);
            deviceInforStream.Append("DeviceName:" + DeviceInfo.DeviceName);
            deviceInforStream.Append("VersionString:" + DeviceInfo.GenerateAppId());
            deviceInforStream.Append("Platform:" + DeviceInfo.Platform);
            deviceInforStream.Append("Idiom:" + DeviceInfo.Idiom.ToString());
            //deviceInforStream.Append("DeviceType:" + DeviceInfo.DeviceType.ToString());
            return deviceInforStream.ToString();
        }

    }
}
