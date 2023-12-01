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
using Newtonsoft.Json.Linq;
using OxyPlot;
using OxyPlot.Series;


namespace WeatherApp
{

    // Classe gestionnaire des villes pour l'affiche d'informations
    public class CityManager
    {
        public List<string> Cities { get; set; }
        public ComboBox City_CB { get; set; }
        public CityManager() {
            Cities = new List<string>();
        }

        public void AddCity(string city)
        {
            Cities.Add(city);
            City_CB.Items.Add(city);
            City_CB.SelectedItem = city;
        }
        public void RemoveCity(string city)
        {
            Cities.Remove(city);
            City_CB.Items.Remove(city);
            City_CB.SelectedItem = "";
        }
    }

    // Classe pour le graphique des températures
    public class MainViewModel
    {
        public MainViewModel(FcstDay0 forecastDay0, FcstDay1 forecastDay1, FcstDay2 forecastDay2, FcstDay3 forecastDay3, FcstDay4 forecastDay4, bool Max)
        {
            var model = new PlotModel {};
            
            //  Températures max et min
            if (Max)
            {
                var maxTempSeries = new LineSeries
                {
                    Title = "Max Temperature",
                    Color = OxyColors.Red
                };

                maxTempSeries.Points.Add(new DataPoint(0, forecastDay0.tmax));
                maxTempSeries.Points.Add(new DataPoint(1, forecastDay1.tmax));
                maxTempSeries.Points.Add(new DataPoint(2, forecastDay2.tmax));
                maxTempSeries.Points.Add(new DataPoint(3, forecastDay3.tmax));
                maxTempSeries.Points.Add(new DataPoint(4, forecastDay4.tmax));

                model.Series.Add(maxTempSeries);
            }
            else
            {

                var minTempSeries = new LineSeries
                {
                    Title = "Min Temperature",
                    Color = OxyColors.Blue
                };

                minTempSeries.Points.Add(new DataPoint(0, forecastDay0.tmin));
                minTempSeries.Points.Add(new DataPoint(1, forecastDay1.tmin));
                minTempSeries.Points.Add(new DataPoint(2, forecastDay2.tmin));
                minTempSeries.Points.Add(new DataPoint(3, forecastDay3.tmin));
                minTempSeries.Points.Add(new DataPoint(4, forecastDay4.tmin));

                model.Series.Add(minTempSeries);
            }
          
            this.Model = model;
        }

        public PlotModel Model { get; private set; }
    }

    public partial class MainWindow : Window
    {
        // initialisation du gestionnaire de villes
        CityManager cityManager = new CityManager();

        // constructeur
        public MainWindow()
        {
            InitializeComponent();
            Main();
            WeatherAsync();

        }

        // Fonction au démarrage de l'application
        public void Main()
        {
            cityManager.City_CB = City_CB;
            cityManager.AddCity("Annecy");
        }

        // Fonction pour afficher les données météo
        public async void WeatherAsync(bool value=true)
        {
            // Classes pour les données météo
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

            if (cityInfo == null || currentCondition == null || forecastInfo == null || forecastDay == null || forecastDay1 == null || forecastDay2 == null || forecastDay3 == null || forecastDay4 == null)
            {
                MessageBox.Show("Error: one of the objects is null");
                return;
            }
            // 
            Current_Weather.Text = $"Current Weather {currentCondition.hour}";
            TB_City.Text = $"{cityInfo.name}, {cityInfo.country}";
            TB_Coordinates.Text = $"Coordinates: {cityInfo.latitude}, {cityInfo.longitude}";
            TB_Elevation.Text = $"Elevation: {cityInfo.elevation} m";

            // Affichage des données météo sur des différents emplacements
            TB_Temperature.Text = $"{currentCondition.tmp}°C";
            TB_Weekday.Text = $"{forecastDay.day_long}";
            TB_Time.Text = $"{currentCondition.hour}";
            TB_Conditions.Text = $"{currentCondition.condition}";
            TB_Wind.Text = $"{currentCondition.wnd_spd} km/h";
            TB_Humidity.Text = $"{currentCondition.humidity} %";
            TB_Pressure.Text = $"{currentCondition.pressure} hPa";
            TB_FeelsLike.Text = $"{currentCondition.tmp}°C";
            TB_Tmax.Text = $"{forecastDay.tmax}°C";
            TB_Tmin.Text = $"{forecastDay.tmin}°C";
            Today_Temperature_Icon.Source = new BitmapImage(new Uri(currentCondition.icon_big));

            TB_Day1.Text = $"{forecastDay1.day_long}";
            TB_Day2.Text = $"{forecastDay2.day_long}";
            TB_Day3.Text = $"{forecastDay3.day_long}";
            TB_Day4.Text = $"{forecastDay4.day_long}";

            TB_Day1_Details.Text = $"{forecastDay1.tmin}°C to {forecastDay1.tmax}°C";
            TB_Day2_Details.Text = $"{forecastDay2.tmin}°C to {forecastDay2.tmax}°C";
            TB_Day3_Details.Text = $"{forecastDay3.tmin}°C to {forecastDay3.tmax}°C";
            TB_Day4_Details.Text = $"{forecastDay4.tmin}°C to {forecastDay4.tmax}°C";

            Day1_Image.Source = new BitmapImage(new Uri(forecastDay1.icon));
            Day2_Image.Source = new BitmapImage(new Uri(forecastDay2.icon));
            Day3_Image.Source = new BitmapImage(new Uri(forecastDay3.icon));
            Day4_Image.Source = new BitmapImage(new Uri(forecastDay4.icon));

            DataContext = new MainViewModel(forecastDay, forecastDay1, forecastDay2, forecastDay3, forecastDay4,value);



           ImageBrush img = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Ressources/Images/Top_BG/Day.jpg")));

           int hour = Convert.ToInt32(currentCondition.hour.Substring(0, 2));

            if (hour >= 16 && hour <= 18)
            {
                img = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Ressources/Images/Top_BG/Dawn.jpg")));
            }
            else
            {
                img = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Ressources/Images/Top_BG/Night.jpg")));
            }

            Middle_Border.Background = img;


        }

        // Fonction pour récupérer les données météo
        public async Task<Root> GetWeather()
        {
            HttpClient client = new HttpClient();
            try
            {
                string city = City_CB.SelectedItem as string;

                HttpResponseMessage response = await client.GetAsync("https://www.prevision-meteo.ch/services/json/"+city);

                if (response.IsSuccessStatusCode)
                {
                    // Convert the response to the class Root
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
        // Create a button click event where whatever is written on Add city is added to the combobox
        private void AddCity_Click(object sender, RoutedEventArgs e)
        {
            // Check if the city is already in the combobox
            if (City_CB.Items.Contains(AddCity_TB.Text))
            {
                MessageBox.Show("City already in the list");
            }
            else
            {
                cityManager.AddCity(AddCity_TB.Text);
                AddCity_TB.Text = "";
            }
        }

        private void RemoveCity_Click(object sender, RoutedEventArgs e)
        {
            // Check if the city is already in the combobox
            if (City_CB.Items.Contains(AddCity_TB.Text))
            {
                cityManager.RemoveCity(AddCity_TB.Text);
                AddCity_TB.Text = "";
            }
            else
            {
                MessageBox.Show("City not in the list");
            }
        }

        // Create a button click event for max temperature and min temperature
        private void MaxTemp_Click(object sender, RoutedEventArgs e)
        {
            WeatherAsync(true);
        }

        private void MinTemp_Click(object sender, RoutedEventArgs e)
        {
            WeatherAsync(false);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle the selection changed event
            if (City_CB.SelectedItem != null)
            {
                WeatherAsync();
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
