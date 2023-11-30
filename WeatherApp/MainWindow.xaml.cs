using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http; // HttpClient
using Newtonsoft.Json;
using System.Threading; // JsonConvert

namespace WeatherApp
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        _:    WeatherAsync();
        }

        public async void WeatherAsync()
        {

           try
            {
                Root root = await GetWeather();
                if (root == null)
                {
                    MessageBox.Show("Error: root is null");
                    return;
                }
                CityInfo cityInfo = root.city_info;
                CurrentCondition currentCondition = root.current_condition;
                ForecastInfo forecastInfo = root.forecast_info;
                FcstDay0 forecastDay = root.fcst_day_0;
                FcstDay1 forecastDay1 = root.fcst_day_1;
                FcstDay2 forecastDay2 = root.fcst_day_2;
                FcstDay3 forecastDay3 = root.fcst_day_3;
                FcstDay4 forecastDay4 = root.fcst_day_4;

                // City Info
                TB_City.Text = cityInfo.name + cityInfo.country;
                MessageBox.Show(cityInfo.name + cityInfo.country);
            }
            catch (Exception e)
            {
                // SHow the exception
                MessageBox.Show(e.ToString());
            }
        }


        public async Task<Root> GetWeather()
        {
            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://www.prevision-meteo.ch/services/json/Annecy");

                if (response.IsSuccessStatusCode)
                {
                    // Convert the response to the class Root
                    var responseBody = await response.Content.ReadAsStringAsync();
                    
                    // Print out the response body to the console
                    Console.WriteLine(responseBody);

                    Root root = JsonConvert.DeserializeObject<Root>(responseBody);

                    return root;
                }
                else
                {
                    // Print out the status code to the console
                    MessageBox.Show($"Unsuccessful status code: {response.StatusCode}");
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        // Close the window
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
