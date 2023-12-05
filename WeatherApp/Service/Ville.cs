using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows;

namespace WeatherApp.Service
{
    public class Ville
    {
        List<string> LsVille;
        public ComboBox City_CB { get; set; }
        string PathVille = @"Ressources/Favourite_Villes.txt";
        // Constructeur
        public Ville()
        {
            LsVille = new List<String>();
        }
        // Fonction au démarrage de l'application
        public void InitializeVilles()
        {
            string[] CitiesList = File.ReadAllLines(PathVille);

            LsVille.Clear();

            foreach (string city in CitiesList)
            {
               if (city.Length == 0) continue;
               AjouterVille(city);
            }
            City_CB.SelectedIndex = 0;
        }
        // Fonction pour mettre à jour la liste des villes
        public void UpdateCity_CB()
        {
            City_CB.Items.Clear();
            foreach (string city in LsVille)
            {
                City_CB.Items.Add(city);
            }
        }
        // Fonction pour ajouter une ville
        public void AjouterVille(string ville)
        {
            LsVille.Add(ville);
            UpdateCity_CB();
            // Select the last item
            City_CB.SelectedIndex = City_CB.Items.Count - 1;
        }
        // Fonction pour récupérer la liste des villes
        public List<string> GetVilles()
        {
            return LsVille;
        }
        // Fonction pour supprimer une ville
        public void SupprimerVille(string ville)
        {
            LsVille.Remove(ville);
            UpdateCity_CB();
            City_CB.SelectedIndex = 0;
        }
        // Fonction pour sauvegarder la liste des villes
        public bool SaveVille()
        {
          try
            {
                File.WriteAllLines(PathVille, LsVille);
                return true;
            }
          catch (Exception e)   
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

    }
}
