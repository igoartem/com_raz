using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OracleClient;


namespace Формы_Сучкова
{
    public partial class R_tovar : Form
    {
        OracleCommand cmd1;
        OracleConnection con1;
        OracleDataReader dr1;

        public R_tovar()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name;

        }

        private void R_tovar_Load(object sender, EventArgs e)
        {
            StreamReader sr;
            string s = "localhost";
            try
            {
                if ((sr = new StreamReader(@"ip_base.txt")) != null)
                     s = sr.ReadLine();
            }

            catch (Exception)
            {
                MessageBox.Show("Error Base!");
            }
            con1 = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = "+s+")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = xe))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd1 = new OracleCommand("", con1);
            con1.Open();
            cmd1.CommandText = "SELECT * from CATEGORY";
            dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                comboBox1.Items.Add(dr1[1].ToString());
            }

            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = -1;
            comboBox2.Items.Clear();
            
            string name = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            int pk=0;
            cmd1.CommandText = "SELECT * from CATEGORY";
            dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                if (dr1[1].ToString() == name)
                    pk = Convert.ToInt32( dr1[0]);
            }

            cmd1.CommandText = "select * from SUBCATEGORY";

            dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                if (Convert.ToInt32( dr1[1]) == pk )
                  comboBox2.Items.Add(dr1[2].ToString());
            }

        }
    }
}
