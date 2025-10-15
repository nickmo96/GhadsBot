using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhadsBot.Database
{
    public class DBConnection
    {
        private readonly string _connectionString = "Data Source=localhost;Initial Catalog = GhadsBot; User ID = sa; Password=***********;Trust Server Certificate=True";
        public DBConnection(string _connectionString) { 
            this._connectionString = _connectionString;
        }

        

    }
}
