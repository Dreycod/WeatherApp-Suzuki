using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WeatherApp.Service
{
    class WeatherAPI
    {
        public ComboBox City_CB { get; set; }
        public WeatherAPI()
        {

        }   
        public async Task<Root> GetWeather()
        {
            HttpClient client = new HttpClient();
            try
            {
                string city = City_CB.SelectedItem as string;

                HttpResponseMessage response = await client.GetAsync("https://www.prevision-meteo.ch/services/json/" + city);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Root root = JsonConvert.DeserializeObject<Root>(responseBody);
                    return root;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
