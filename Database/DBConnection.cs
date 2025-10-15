using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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

        public DBConnection TryConnection()
        {
using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Forbindelse til databasen lykkedes.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Forbindelse til databasen mislykkedes: " + ex.Message);
                }
            }
            return this;
        }
    }
}
