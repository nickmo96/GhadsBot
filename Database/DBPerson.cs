using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhadsBot.Database
{
    public class DBPerson
    {
        private readonly string _getAllQuery = "SELECT ChatId, FirstName, LastName, Username FROM Person";
        private readonly string _insertQuery = "INSERT INTO Person (ChatId, FirstName, LastName, Username) VALUES (@ChatId, @FirstName, @LastName, @Username)";
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LocalConnection"].ConnectionString.ToString();
        
        public DBPerson()
        {
          
        }


        public IEnumerable<Person> GetAllPersons()
        {
            List<Person> persons = new List<Person>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = _getAllQuery;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        long chatId = reader.GetInt64(reader.GetOrdinal("ChatId"));
                        string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                        string username = reader.GetString(reader.GetOrdinal("Username"));
                        Person p = new Person(chatId, firstName , lastName, username);
                        persons.Add(p);
                    }
                }
            }

            return persons;
        }
        public void InsertPerson(Person person) //lave transaction?
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = _insertQuery;

                    cmd.Parameters.AddWithValue("@ChatId", person.ChatId);
                    cmd.Parameters.AddWithValue("@FirstName", person.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", person.LastName);
                    cmd.Parameters.AddWithValue("@Username", person.Username);
                    cmd.ExecuteNonQuery();


                }
            }
        }
    }
}

