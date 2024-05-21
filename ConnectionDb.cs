using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;



namespace Im_Not_Playing
{
    internal class ConnectionDb
    {
        string connectString = string.Format("Host={0};Database={1};Username ={2};Password={3};", "localhost", "FaceDetect", "postgres", "guswhd23");

        public bool ConnectionTest()
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectString))
                {
                    conn.Open();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetConnectionString()
        {
            return connectString;
        }

    }
}
