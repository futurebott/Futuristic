using Futuristic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Futuristic.Services
{
    public sealed class UserService : BaseService<Application>
    {
       
        private static readonly Lazy<UserService> lazy = new Lazy<UserService>(() => new UserService());
        public static Application Application;
        public static UserService Instance { get { return lazy.Value; } }
        private UserService() : base("Application")
        {
            Application = new Application();
            string infoStream = GetDeviceInfoStream();
            Task.Run(async () => {
                Application = await this.AddUpdateEntity(Application);
            });
          
        }
        public  Guid GetApplicationId()
        {
            return Application.ApplicationId;
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
            
           
            // Device Model (SMG-950U, iPhone10,6)
            StringBuilder deviceInforStream = new StringBuilder();
            deviceInforStream.Append("Model:" + DeviceInfo.Model);
            deviceInforStream.Append(",Manufacturer:" + DeviceInfo.Manufacturer);
            deviceInforStream.Append(",DeviceName:" + DeviceInfo.Name);
            deviceInforStream.Append(",VersionString:" + DeviceInfo.VersionString);
            deviceInforStream.Append(",Platform:" + DeviceInfo.Platform);
            deviceInforStream.Append(",Idiom:" + DeviceInfo.Idiom.ToString());
            deviceInforStream.Append(",DeviceType:" + DeviceInfo.DeviceType.ToString());
            return deviceInforStream.ToString();
        }

    }
}
