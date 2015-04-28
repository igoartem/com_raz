using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.IO;

namespace Формы_Сучкова
{
    public partial class Add_redact_worker : Form
    {
        int rejim;
        int pk;
        OracleCommand cmd_ra;     //
        OracleConnection con_ra;  // Подключение для вывода товаров в таблицу
        OracleDataReader dr_ra;   //

        public Add_redact_worker(int rej, int _pk)
        {
            rejim = rej;
            pk = _pk;
            InitializeComponent();
        }

        public void connect()
        {
            StreamReader sr;
            string s = null;
            try
            {
                if ((sr = new StreamReader(@"ip_base.txt")) != null)
                    s = sr.ReadLine();
            }

            catch (Exception)
            {
                MessageBox.Show("Error Base!");
            }

            con_ra = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + s + ")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = XE))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd_ra = new OracleCommand("", con_ra);
            con_ra.Open();
        }

        private void redact(int pk)
        {
            button1.Text = "Сохранить";
            cmd_ra.CommandText = "SELECT fio, login, password, status from worker where pk_worker = " + pk.ToString();
            dr_ra = cmd_ra.ExecuteReader();
            dr_ra.Read();
            textBox1.Text = dr_ra[0].ToString();
            textBox2.Text = dr_ra[1].ToString();
            textBox3.Text = dr_ra[2].ToString();
            comboBox1.SelectedIndex = Convert.ToInt32(dr_ra[3].ToString());
        }

        private void add()
        {
            button1.Text = "Добавить";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Add_redact_worker_Load(object sender, EventArgs e)
        {
            connect();
            switch (rejim)
            {
                case 1: { redact(pk); }; break;
                case 0: { add(); }; break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rejim == 0)
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || comboBox1.Text == "")
                {
                    MessageBox.Show("Заполните все поля");
                }
                else
                {
                    string sss = "insert into worker (FIO, LOGIN, PASSWORD, STATUS) values('" + textBox1.Text.ToString() + "','" + textBox2.Text.ToString() + "','" + textBox3.Text.ToString() + "','" + comboBox1.SelectedIndex.ToString() + "')";
                    cmd_ra.CommandText = sss;
                    cmd_ra.ExecuteNonQuery();
                    this.Close();
                }
            }
            else
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || comboBox1.Text == "")
                {
                    MessageBox.Show("Заполните все поля");
                }
                else
                {
                    string sss = "UPDATE worker SET FIO = '" + textBox1.Text + "', LOGIN = '" + textBox2.Text + "', PASSWORD = '" + textBox3.Text + "', status = '" + comboBox1.SelectedIndex + "' where pk_worker = " + pk.ToString();
                    cmd_ra.CommandText = sss;
                    cmd_ra.ExecuteNonQuery();
                    this.Close();
                }
            }
        }
    }
}
