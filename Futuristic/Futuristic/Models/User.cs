using Futuristic.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Futuristic.Models
{
    public class User
    {
        public Guid ApplicationId { get; set; }
        public double CurrentLat { get; set; }
        public double CurrentLong { get; set; }
        public DateTime TimeStamp { get; set; }
        public TimeZoneInfo CurrentTimeZone { get; set; }
        public string Name { get; set; }
        public int LocationRefreshIntervalinMinutes { get; set; }
        public User()
        {
            ApplicationId = new Guid("00000000-0000-0000-0000-000000000000"); //new Guid("44d0c89a-6c3c-4f20-ac05-e0b1af1cce4a");
            CurrentLat = 0;
            CurrentLong = 0;
            TimeStamp = DateTime.Now;
        }
       
       
    }
}
