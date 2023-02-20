using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace ADO_D1
{
    public partial class DisconnectedMode : Form
    {
        SqlConnection connection;
        DataTable dataTable;
        public DisconnectedMode()
        {
            InitializeComponent();
            // Define connection
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString);
        }

        private void DisconnectedMode_Load(object sender, EventArgs e)
        {
            // Define command
            //SqlCommand command = new SqlCommand("select * from topic",connection);

            //// Define DataAdaptor link select command
            //SqlDataAdapter adapter = new SqlDataAdapter();
            //adapter.SelectCommand = command;
            SqlDataAdapter adapter = new SqlDataAdapter("select * from topic", connection);

            // Define DataTable
            dataTable = new DataTable();

            // Load Data
            adapter.Fill(dataTable);
            dataTable.Columns[0].AllowDBNull = false;
            dataTable.Columns[0].Unique = true;

            dgv_topics.DataSource = dataTable;
            dgv_topics.Columns[0].ReadOnly = true;
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            DataRow dataRow = dataTable.NewRow();
            dataRow[0] = nud_id.Value;
            dataRow[1] = txt_name.Text;

            dataTable.Rows.Add(dataRow);

            MessageBox.Show("Data have been added.");
        }

        private void dgv_topics_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            nud_id.Value = (int)dgv_topics.SelectedRows[0].Cells[0].Value;
            txt_name.Text = dgv_topics.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            int id = (int)nud_id.Value;

            foreach(DataRow row in dataTable.Rows)
            {
                if (row.RowState != DataRowState.Deleted && (int)row["Top_id"] == id) row["Top_name"] = txt_name.Text;
            }

            MessageBox.Show("Data Updated Successfully");
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            int id = (int)nud_id.Value;

            List<string> strings = new List<string>()
            {
                "ola","Mona","Hoda"
            };
            //foreach (var item in strings)
            //{
            //    if (item == "Mona") strings.Remove("Mona");
            //}
            foreach (DataRow row in dataTable.Rows)
            {
                if ((int)row["Top_id"] == id) row.Delete();
            }

            MessageBox.Show("Data deleted Successfully");
        }

        private void btn_saveChanges_Click(object sender, EventArgs e)
        {
            foreach(DataRow row in dataTable.Rows)
            {
                Debug.WriteLine(row.RowState);
            }

            // Define connection -- global
            // Define command

            // update
            SqlCommand updateCommand = new SqlCommand("update Topic set Top_Name = @name where Top_Id = @id", connection);
            updateCommand.Parameters.Add("name", SqlDbType.NVarChar, 50, "top_name");
            updateCommand.Parameters.Add("id", SqlDbType.Int, 4, "top_id");

            // insert
            // without identity
            SqlCommand insertCommand = new SqlCommand("insert into Topic values(@id,@name)", connection);
            insertCommand.Parameters.Add("name", SqlDbType.NVarChar, 50, "top_name");
            insertCommand.Parameters.Add("id", SqlDbType.Int, 4, "top_id");

            // with identity
            //SqlCommand insertCommand = new SqlCommand("insert into Topic values(@name)", connection);
            //insertCommand.Parameters.Add("name", SqlDbType.NVarChar, 50, "top_name");

            // delete
            SqlCommand deleteCommand = new SqlCommand("delete from Topic where Top_Id = @id", connection);
            deleteCommand.Parameters.Add("id", SqlDbType.Int, 4, "top_id");

            // Define DataAdaptor + link commands
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.UpdateCommand = updateCommand;
            adapter.DeleteCommand = deleteCommand;
            adapter.InsertCommand = insertCommand;

            adapter.Update(dataTable);
        }
    }
}
