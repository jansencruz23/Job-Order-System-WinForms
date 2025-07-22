using Guna.UI2.Licensing.LightJson.Serialization;
using Job_Order_System.Data;
using Job_Order_System.Forms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Job_Order_System
{
    static class Program
    {
        static string ConfigPath => "server.config";

        static DbConfig LoadConfig()
        {
            if (!File.Exists(ConfigPath))
            {
                return null;
            }
            var json = File.ReadAllText(ConfigPath);
            return JsonSerializer.Deserialize<DbConfig>(json);
        }

        static void SaveConfig(DbConfig config)
        {
            var json = JsonSerializer.Serialize(config);
            File.WriteAllText("server.config", json);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var config = LoadConfig();
            bool isValidServer = false;

            while (!isValidServer)
            {
                if (config == null)
                {
                    using (var serverForm = new Connect())
                    {
                        if (serverForm.ShowDialog() == DialogResult.OK)
                        {
                            config = new DbConfig
                            {
                                Server = serverForm.SelectedServer,
                                User = serverForm.User,
                                Password = serverForm.Password,
                            };

                            Database.SetConnectionString(config.Server, config.User, config.Password);
                            SaveConfig(config);
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                try
                {
                    Database.SetConnectionString(config.Server, config.User, config.Password);
                    var tempConnection = new MySqlConnection(Database.CONNECTION_STRING);
                    tempConnection.Open();
                    tempConnection.Close();
                    isValidServer = true;
                }
                catch
                {
                    File.Delete(ConfigPath); // Remove invalid config
                    config = null; // Force re-prompt
                }
            }

            Application.Run(new Login());
        }
    }
}