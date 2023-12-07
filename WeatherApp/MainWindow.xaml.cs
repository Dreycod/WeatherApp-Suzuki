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
using System.IO;
using System.Reflection.Metadata;
using WeatherApp.Service;

namespace WeatherApp
{
    public partial class MainWindow : Window
    {
        Ville ville;
        Client client;
        WeatherAPI weatherAPI;
        // constructeur
        public MainWindow()
        {
            InitializeComponent();

            ville = new Ville();
            ville.City_CB = City_CB;

            client = new Client();
            client.TB_Greetings = TB_Greetings;

            weatherAPI = new WeatherAPI();
            weatherAPI.City_CB = City_CB;

            ville.InitializeVilles();
            client.InitializeClient();
            WeatherAsync();

        }

        // Fonction pour afficher les données météo
        public async void WeatherAsync(bool value=true)
        {
            // Classes pour les données météo
            Root root = await weatherAPI.GetWeather();

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
                ville.SupprimerVille(City_CB.SelectedItem as string);
                return;
            }
            // 
            TB_City.Text = $"{cityInfo.name}, {cityInfo.country}";
            TB_Coordinates.Text = $"Coordinates: {cityInfo.latitude}, {cityInfo.longitude}";
            TB_Elevation.Text = $"Elevation: {cityInfo.elevation} m";

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
            else if (hour >= 18)
            {
                img = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Ressources/Images/Top_BG/Night.jpg")));
            }

            Middle_Border.Background = img;
        }

        // Events

        // évenement pour ajouter une ville
        private void AddCity_Click(object sender, RoutedEventArgs e)
        {
            if (City_CB.Items.Contains(AddCity_TB.Text))
            {
                MessageBox.Show("City already in the list");
            }
            else
            {
                ville.AjouterVille(AddCity_TB.Text);
                AddCity_TB.Text = "";
            }
        }

        // évenement pour supprimer une ville
        private void RemoveCity_Click(object sender, RoutedEventArgs e)
        {
            if (City_CB.Items.Contains(AddCity_TB.Text))
            {
                ville.SupprimerVille(AddCity_TB.Text);
                AddCity_TB.Text = "";
            }
            else
            {
                MessageBox.Show("City not in the list");
            }
        }

        // évenement pour afficher la température maximale
        private void MaxTemp_Click(object sender, RoutedEventArgs e)
        {
            WeatherAsync(true);
        }

        // évenement pour afficher la température minimale
        private void MinTemp_Click(object sender, RoutedEventArgs e)
        {
            WeatherAsync(false);
        }

        // évenement pour afficher les données météo
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (City_CB.SelectedItem != null)
            {
                WeatherAsync();
            }
        }

        // évenement pour déplacer la fenêtre
        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        // évenement pour fermer la fenêtre
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // évenement sauvegarder les villes
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // the program waits until the saveville() returns true
            while (!ville.SaveVille())
            {
                Thread.Sleep(100);
            }
            
        }
    }
}
