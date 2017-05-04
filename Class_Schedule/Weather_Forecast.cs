using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Net.Http;
using System.IO;

namespace Class_Schedule
{
    class Weather_Forecast
    {
        public async static Task<RootObject1> GetWeather(string city)
        {
            var http = new HttpClient();
            var response = await http.GetAsync("http://api.openweathermap.org/data/2.5/forecast?q=" + city + "&appid=b6df5a092ff3dac33cb8e907d90f1506");
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObject1));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject1)serializer.ReadObject(ms);
            return data;
        }
    }
    [DataContract]
    public class Main1
    {
        [DataMember]
        public double temp { get; set; }
        [DataMember]
        public double temp_min { get; set; }
        [DataMember]
        public double temp_max { get; set; }
        [DataMember]
        public double pressure { get; set; }
        [DataMember]
        public double sea_level { get; set; }
        [DataMember]
        public double grnd_level { get; set; }
        [DataMember]
        public int humidity { get; set; }
        [DataMember]
        public double temp_kf { get; set; }
    }
    [DataContract]
    public class Weather1
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string main { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string icon { get; set; }
    }
    [DataContract]
    public class Clouds1
    {
        [DataMember]
        public int all { get; set; }
    }
    [DataContract]
    public class Wind1
    {
        [DataMember]
        public double speed { get; set; }
        [DataMember]
        public double deg { get; set; }
    }
    [DataContract]
    public class Rain1
    {
        [DataMember]
        public double __invalid_name__3h { get; set; }
    }
    [DataContract]
    public class Sys1
    {
        [DataMember]
        public string pod { get; set; }
    }
    [DataContract]
    public class List1
    {
        [DataMember]
        public int dt { get; set; }
        [DataMember]
        public Main1 main { get; set; }
        [DataMember]
        public List<Weather1> weather { get; set; }
        [DataMember]
        public Clouds1 clouds { get; set; }
        [DataMember]
        public Wind1 wind { get; set; }
        [DataMember]
        public Rain1 rain { get; set; }
        [DataMember]
        public Sys1 sys { get; set; }
        [DataMember]
        public string dt_txt { get; set; }
    }
    [DataContract]
    public class Coord1
    {
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public double lon { get; set; }
    }
    [DataContract]
    public class City1
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public Coord1 coord { get; set; }
        [DataMember]
        public string country { get; set; }
    }
    [DataContract]
    public class RootObject1
    {
        [DataMember]
        public string cod { get; set; }
        [DataMember]
        public double message { get; set; }
        [DataMember]
        public int cnt { get; set; }
        [DataMember]
        public List<List1> list { get; set; }
        [DataMember]
        public City1 city { get; set; }
    }

}
