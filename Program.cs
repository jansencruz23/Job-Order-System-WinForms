using Job_Order_System.Data;
using Job_Order_System.Forms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Job_Order_System
{
    static class Program
    {
        static string LoadServerName()
        {
            string path = "server.config";
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }

        static void SaveServerName(string serverName)
        {
            File.WriteAllText("server.config", serverName);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string serverName = LoadServerName();
            bool isValidServer = false;

            while (!isValidServer)
            {
                if (string.IsNullOrEmpty(serverName))
                {
                    using (var serverForm = new Connect())
                    {
                        if (serverForm.ShowDialog() == DialogResult.OK)
                        {
                            serverName = serverForm.SelectedServer;
                            Database.SetConnectionString(serverName);
                            SaveServerName(serverName);
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                try
                {
                    Database.SetConnectionString(serverName);
                    var connectionStringTemp = Database.CONNECTION_STRING;
                    var tempConnection = new MySqlConnection(connectionStringTemp);
                    tempConnection.Open();
                    tempConnection.Close();
                    isValidServer = true;
                }
                catch
                {
                    File.Delete("server.config"); // Remove invalid config
                    serverName = null; // Force re-prompt
                }
            }

            Application.Run(new Login());
        }
    }
}