using System.Configuration;
using System.Data.SqlClient;

namespace ADO_Core
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string connectionstring = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionstring);
        }
    }
}