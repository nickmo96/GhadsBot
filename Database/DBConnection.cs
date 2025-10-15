using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;


namespace GhadsBot.Database
{
    public class DBConnection
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["LocalConnection"].ConnectionString.ToString();
        private static DBConnection? _instance;
        public DBConnection() { 
            
        }

        
        public DBConnection GetInstance() {
            if(_instance == null) {
                _instance = new DBConnection();
            }
            return _instance;
        }
    }
}
