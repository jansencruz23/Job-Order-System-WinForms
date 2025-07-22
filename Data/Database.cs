using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Order_System.Data
{
    public static class Database
    {
        public static void SetConnectionString(string server, string user, string password)
        {
            CONNECTION_STRING = $"Server={server};Database=joborder_winforms;User={user};Password={password};CharSet=utf8mb4;";
        }

        public static string CONNECTION_STRING;
    }
}