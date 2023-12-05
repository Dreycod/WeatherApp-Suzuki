using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;

namespace WeatherApp.Service
{
    class Client
    {
        private string Client_Info = @"Ressources/Client_Info.txt";
        public TextBlock TB_Greetings { get; set; }
        public Client()
        {
            
        }

        public void InitializeClient()
        {
            string[] ClientInfo = File.ReadAllLines(Client_Info);

            if (ClientInfo[1] == "Homme")
            {
                TB_Greetings.Text = $"Welcome Mr {ClientInfo[0]}, Look at the weather!";
            }
            else
            {
                TB_Greetings.Text = $"Welcome Miss {ClientInfo[0]} Look at the weather!";
            }
        }

    }
}
