using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ADO_D1
{
    public partial class ConnectedMode : Form
    {
        private SqlConnection connection;
        public ConnectedMode()
        {
            InitializeComponent();
            //connection = new SqlConnection("Data Source=.;Initial Catalog=ITI;Integrated Security=True");
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DisplayEditDelete(false);
            FillStudentData();

            #region department
            dgv_students.Columns[2].Visible= false;
            //connection            #region Department
            // connection -> ready
            // Command
            SqlCommand command1 = new SqlCommand("select Dept_Id,Dept_Name from Department", connection);

            // open connection
            connection.Open();

            // execute command
            SqlDataReader dataReader1 = command1.ExecuteReader();

            // prepare data
            List<Department> departments = new List<Department>();
            while (dataReader1.Read())
            {
                Department department = new Department();
                department.Id = (int)dataReader1[0];
                department.Name = dataReader1[1].ToString();
                departments.Add(department);
            }
            // close connection
            connection.Close();

            cb_department.DataSource = departments;
            cb_department.DisplayMember = "Name";
            cb_department.ValueMember = "Id";
            #endregion
        }

        private void FillStudentData()
        {
            #region students
            //Define Connection
            //SqlConnection connection = new SqlConnection();
            //string connectionString = "Data Source=.;Initial Catalog=ITI;Integrated Security=True";
            //connection.ConnectionString = connectionString;

            // SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=ITI;Integrated Security=True");
            //Define Query/Command
            //SqlCommand command = new SqlCommand();
            //command.CommandType = CommandType.Text;
            //command.CommandText = "Select * from student";
            //command.Connection = connection;

            SqlCommand command = new SqlCommand("Select * from student", connection);
            //Open connection
            connection.Open();

            //Execute Query
            SqlDataReader dataReader = command.ExecuteReader();

            //prepare data
            List<Student> students = new List<Student>();

            while (dataReader.Read())
            {
                Student student = new Student();
                student.St_Id = (int)dataReader[0];
                student.St_Fname = dataReader[1].ToString();
                student.ST_Lname = dataReader[2].ToString();
                student.St_Address = dataReader[3].ToString();
                if (!Convert.IsDBNull(dataReader[4]))
                {
                student.St_Age = (int)dataReader[4];

                }
                student.Dept_Id = (int)dataReader[5];
                student.St_super = (int)dataReader[6];
                students.Add(student);
            }
            //Close connection
            connection.Close();

            dgv_students.DataSource = students;

            cb_supervisor.DataSource = students;
            cb_supervisor.DisplayMember = "St_Fname";
            cb_supervisor.ValueMember = "St_Id";
            #endregion

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private int GetLastStudentID()
        {
            // define command
            SqlCommand command = new SqlCommand("select Max(st_id) from Student", connection);

            // open connection
            connection.Open();
            // execute command
            var result = command.ExecuteScalar();
            // prepare data
            int id = (int)result;
            //close connection
            connection.Close();

            return id;
        }
        private void btn_Add_Click(object sender, EventArgs e)
        {

            if (txt_lname.Text == "" || txt_fname.Text == "" || (txt_address.Text == string.Empty) || nud_age.Value == 0)
            {
                MessageBox.Show("Fill all Data");
            }
            else
            {
                // define connection

                // define command
                int id = GetLastStudentID() + 1;
                //string commandText = "insert into Student (St_Id,St_Fname,St_Lname,St_Address, St_Age,Dept_Id,St_super) " +
                //    "values(" + id + ",'" + txt_fname.Text + "','" + txt_lname.Text + "','" + txt_address.Text + "'," + nud_age.Value + "," + cb_department.SelectedValue + "," + cb_supervisor.SelectedValue + ")";
                string commandText = "insert into student values(@id,@fname,@lname,@address,@age,@dept,@super)";
                
                
                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("fname", txt_fname.Text);
                command.Parameters.AddWithValue("lname", txt_lname.Text);
                command.Parameters.AddWithValue("address", txt_address.Text);
                command.Parameters.AddWithValue("age",nud_age.Value);
                command.Parameters.AddWithValue("dept", cb_department.SelectedValue);
                command.Parameters.AddWithValue("super",cb_supervisor.SelectedValue);

                // open connection
                connection.Open();
                // execute connection
                int result = command.ExecuteNonQuery();
                // prepare data

                // close connection
                connection.Close();

                if (result > 0)
                {
                    MessageBox.Show("Data inserted successfully");
                    FillStudentData();
                    ClearInputs();

                }
            }
        }

        private int id = 0;
        private void dgv_students_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DisplayEditDelete(true);
            id = (int)dgv_students.SelectedRows[0].Cells[0].Value;
            txt_fname.Text = dgv_students.SelectedRows[0].Cells[1].Value.ToString();
            txt_lname.Text = dgv_students.SelectedRows[0].Cells[2].Value.ToString();
            txt_address.Text = dgv_students.SelectedRows[0].Cells[3].Value.ToString();
            nud_age.Value = (int)dgv_students.SelectedRows[0].Cells[4].Value;
            cb_department.SelectedValue = (int)dgv_students.SelectedRows[0].Cells[5].Value;
            cb_supervisor.SelectedValue = (int)dgv_students.SelectedRows[0].Cells[6].Value;
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            // define command
            SqlCommand command = new SqlCommand("Update student set st_Fname = @fname, St_Lname = @lname,St_Address=@address,St_Age=@age,Dept_Id=@dept,St_super=@super where St_Id = @id");
            command.Connection = connection;
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("fname", txt_fname.Text);
            command.Parameters.AddWithValue("lname", txt_lname.Text);
            command.Parameters.AddWithValue("address", txt_address.Text);
            command.Parameters.AddWithValue("age", nud_age.Value);
            command.Parameters.AddWithValue("dept", cb_department.SelectedValue);
            command.Parameters.AddWithValue("super", cb_supervisor.SelectedValue);

            //open connection
            connection.Open();

            //execute
            int result = command.ExecuteNonQuery();
            //close connection
            connection.Close();

            if(result > 0)
            {
                MessageBox.Show("Data Updated Successfully");
                FillStudentData();
                DisplayEditDelete(false);
                ClearInputs();

            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            //define command
            SqlCommand commmand = new SqlCommand("delete from student where St_id = @id", connection);
            commmand.Parameters.Add(new SqlParameter("id", id));

            // open connection
            connection.Open();

            //execute
            int result = commmand.ExecuteNonQuery();

            //close connection
            connection.Close();

            if(result > 0)
            {
                MessageBox.Show("Data deleted successfully");
                FillStudentData();
                DisplayEditDelete(false);
                ClearInputs();
            }
        }

        private void DisplayEditDelete(bool visability)
        {
            btn_delete.Visible = visability;
            btn_update.Visible = visability;
            btn_Add.Visible = !visability;
        }

        private void ClearInputs()
        {
            txt_address.Text = txt_fname.Text = txt_lname.Text = "";
            nud_age.Value = 0;
        }
    }
}
