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
    public class UserService 
    {
       
        //private static readonly Lazy<UserService> lazy = new Lazy<UserService>(() => new UserService());
        public static Application Application;
        //private ApplicationService _appService;
        //public static UserService Instance { get { return lazy.Value; } }
        //private UserService() 
        //{
        //    Init();
        //}
       public static void Init()
        {
            var _appService = new ApplicationService();
            Application = new Application();
            string infoStream = Utilities.GetDeviceInfo();
            Application.DeviceId = infoStream;
            Utilities.SetLastRefreshTime(DateTime.Now.AddMinutes(-100)); // this will set the initial
            Task.Run(async () =>
            {
                Application.CurrentLat = 1; // unable to get location here so will update on another call
                Application.CurrentLong = 1;
                Application = await _appService.AddUpdateEntity(Application);
            });
        }
        public static Guid GetApplicationId()
        {
            return Application.ApplicationId;
        }
        
        public static async Task<Location> CurrentLocation()
        {
            if (Utilities.Does_App_Needs_A_Refresh())
            {
                var currentLocation = new Location();
                try
                {
                    var locationManager = new LocationMonanager();
                    currentLocation = await locationManager.GetLocationCache();
                    Utilities.SetLastRefreshTime(DateTime.Now);
                    Utilities.SetLocation(currentLocation);
                    //update last access point for application
                    if (Utilities.GetAppKey("ApplocationAdded") != "Yes")
                    {
                        var _appService = new ApplicationService();
                        Application = new Application();
                        string infoStream = Utilities.GetDeviceInfo();
                        Application.DeviceId = infoStream;
                        Application.CurrentLat = currentLocation.Latitude;
                        Application.CurrentLong = currentLocation.Longitude;
                        _appService.AddUpdateEntity(Application).ConfigureAwait(false);
                        Utilities.SetAppKey("ApplocationAdded", "Yes");
                    }
                    return currentLocation;
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return Utilities.GetLocation();
            }
           

        }
    
      
        public  string LoadApplicationId()
        {
            string applicationIdString = string.Empty;
            var appFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Application.info");
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
