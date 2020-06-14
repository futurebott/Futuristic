using Futuristic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Linq;

namespace Futuristic.Services
{
    public sealed class UserService 
    {
       
        private static readonly Lazy<UserService> lazy = new Lazy<UserService>(() => new UserService());
        public static Application Application;
        private ApplicationService _appService;
        public static UserService Instance { get { return lazy.Value; } }
        private UserService() 
        {
            Init();
        }
       public void Init()
        {
            _appService = new ApplicationService();
            Application = new Application();
            string infoStream = GetDeviceInfo();
            Application.DeviceId = infoStream;
            Task.Run(async () =>
            {
                //var curLoc = await this.CurrentLocation();
                Application.CurrentLat = 1; // unable to get location here so will update on another call
                Application.CurrentLong =1;
                Application = await _appService.AddUpdateEntity(Application);
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
        public string AppKey()
        {
            try {
                string appkEy = string.Empty;
                Task.Run(async () => { appkEy = await SecureStorage.GetAsync("appKey"); });
                if(string.IsNullOrEmpty(appkEy))
                {
                    appkEy = Guid.NewGuid().ToString();
                    Task.Run(async () => { await SecureStorage.SetAsync("appKey", appkEy); });
                }
                return appkEy;
            }
            catch(Exception ex)
            {
                throw;
            }
           
        }
        public string GetDeviceInfo()
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
        public  string LoadApplicationId()
        {
            string applicationIdString = string.Empty;
            var appFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Application.txt");
            if (appFile == null || !File.Exists(appFile))
            {
                var newGuid = Guid.NewGuid();
                using (var writer = File.CreateText(appFile))
                {
                    writer.Write("appId:" + newGuid);
                }
                applicationIdString = newGuid.ToString();
            }
            else
            {
                using (var reader = new StreamReader(appFile, true))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var AplicationIdstring = line.Split(':');
                            if (AplicationIdstring.Length > 0)
                                applicationIdString = AplicationIdstring[1];
                        }
                    }
                }
            }
            return applicationIdString;
            

        }

    }
}
