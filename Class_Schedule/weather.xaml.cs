using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Class_Schedule.ServiceReference1;
using System.Net.Http;
using System.Diagnostics;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Class_Schedule
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class weather : Page
    {
        public weather()
        {
            this.InitializeComponent();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string city_name = get_city_name.Text;
            get_city_name.Text="";
            RootObject myWeather = await OpenWeatherMapProxy.GetWeather(city_name);
            RootObject1 myWeather_Forcast = await Weather_Forecast.GetWeather(city_name);
            try
            {
                Brush brush = new SolidColorBrush(Colors.Black);
                border1.BorderThickness = new Thickness(3,3,3,3);
                border1.CornerRadius = new CornerRadius(5);
                border1.BorderBrush = brush;
                border2.BorderThickness = new Thickness(3,3,3,3);
                border2.CornerRadius = new CornerRadius(5);
                border2.BorderBrush = brush;
                Windows.Web.Http.HttpClient http = new Windows.Web.Http.HttpClient();
                Uri uri = new Uri("http://openweathermap.org/img/w/" + myWeather.weather[0].icon+".png");
                IBuffer buffer = await http.GetBufferAsync(uri);
                BitmapImage tmp_img = new BitmapImage();
                using (IRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    await stream.WriteAsync(buffer);
                    stream.Seek(0);
                    await tmp_img.SetSourceAsync(stream);
                }
                weather_condition.Source = tmp_img;

                city_message.Text = "\n"+myWeather.name+" "+myWeather.coord.lat+" "+myWeather.coord.lon;

                current.Text ="Current\n"+((int)myWeather.main.temp - 273).ToString() + "°C\n" + myWeather.weather[0].description + "\n"
              + "speed " + myWeather.wind.speed.ToString() + "m\\s" ;

                DateTime startTime = new DateTime(1970, 1, 1);
                DateTime now_date = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
                int lowest_tmp = 400;
                int highest_tmp = -400;
                long timeStamp = (long)(now_date - startTime).TotalSeconds+86400;
                for (int i=0;i<myWeather_Forcast.list.Count;++i)
                {
                    if(0<=myWeather_Forcast.list[i].dt-timeStamp&& myWeather_Forcast.list[i].dt-timeStamp<=86400)
                    {
                        if (myWeather_Forcast.list[i].main.temp < lowest_tmp)
                            lowest_tmp = (int)myWeather_Forcast.list[i].main.temp;
                        if (myWeather_Forcast.list[i].main.temp > highest_tmp)
                            highest_tmp = (int)myWeather_Forcast.list[i].main.temp;
                    }
                    if (myWeather_Forcast.list[i].dt - timeStamp > 86400) break;
                }
                Uri uri2 = new Uri("http://openweathermap.org/img/w/" + myWeather.weather[0].icon + ".png");
                IBuffer buffer2 = await http.GetBufferAsync(uri);
                using (IRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    await stream.WriteAsync(buffer);
                    stream.Seek(0);
                    await tmp_img.SetSourceAsync(stream);
                }
                weather_condition_tomorrow.Source = tmp_img;
                tomorrow.Text = "Tomorrow\n"+(lowest_tmp-273).ToString() + "°C-" + (highest_tmp-273).ToString()+ "°C\n" + myWeather_Forcast.list[8].weather[0].description;
            }
            catch
            {
                
            }
        }
    }
    

}
