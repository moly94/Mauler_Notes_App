using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace MyLittleNotesApp.Models
{
    public class Note:ObservableObject
    {
        [JsonConstructor]
        public Note(string content, DateTime date, Geopoint geopoint, double latitude, double longitude)
        {
            Content = content;
            Date = date;
            Geopoint = geopoint;
            Latitude = latitude;
            Longitude = longitude;

        }

        public int Id { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("DateTime")]
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return Content;
        }

        [JsonIgnore]
        public Geopoint Geopoint { get; set; }

        [JsonProperty("Latitude")]
        public double Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
    
    
    }
}
