using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Controls;

namespace WeatherApp.Service
{
    public class Ville
    {
        List<string> LsVille;
        public ComboBox City_CB { get; set; }
        string PathVille = @"Ressources/Favourite_Villes.txt";
        public Ville()
        {
            LsVille = GetVille();

        }

        public void UpdateCity_CB()
        {
            City_CB.Items.Clear();
            foreach (string city in LsVille)
            {
                City_CB.Items.Add(city);
            }
        }

        public void AjouterVille(string ville)
        {
            LsVille.Add(ville);
            UpdateCity_CB();
        }

        public List<string> GetVille()
        {
            string[] CitiesList = File.ReadAllLines(PathVille);
            foreach (string city in CitiesList)
            {
                LsVille.Add(city);
            }
            return LsVille;
        }

        public void SupprimerVille(string ville)
        {
            LsVille.Remove(ville);
            UpdateCity_CB();
            City_CB.Text = "";
        }

        public void SaveVille()
        {
            File.WriteAllLines(PathVille, LsVille);
        }

    }
}
