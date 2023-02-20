using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADO_D1
{
    public partial class Layerd : Form
    {
        public Layerd()
        {
            InitializeComponent();
        }

        private void Layerd_Load(object sender, EventArgs e)
        {
            dgv_topics.DataSource = TopicBusinessLayer.GetAll();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            int result = TopicBusinessLayer.Add((int)nud_id.Value,txt_name.Text) ;
            if(result > 0 ) {
                MessageBox.Show("Data have been added.");
                dgv_topics.DataSource = DatabaseLayer.Select("Select * from topic");
            }
        }
    }
}
