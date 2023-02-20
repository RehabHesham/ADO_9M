using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_D1
{
    internal class DatabaseLayer
    {
        static string connectionString;
        static DatabaseLayer()
        {
            connectionString = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
        }

        public static DataTable Select(string command)
        {
            // Disconnected
            // Define dataAdaptor with command & connection string
            SqlDataAdapter adapter = new SqlDataAdapter(command, connectionString);

            // Define DataTable
            DataTable dataTable = new DataTable();

            // Fill Data 
            adapter.Fill(dataTable);

            return dataTable;
        }

        public static int DMLCommands(string command)
        {
            // Connected
            // define connection
            SqlConnection connection = new SqlConnection(connectionString);

            // define command
            SqlCommand sqlCommand = new SqlCommand(command,connection);

            // open connection
            connection.Open();

            // execute command
            int result = sqlCommand.ExecuteNonQuery();

            //close connection
            connection.Close();

            return result;
        }


    }
}
