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
    public partial class Form1 : Form
    {
        OracleCommand cmd_log;
        OracleConnection con_log;
        OracleDataReader dr_log;

        public Form1()
        {
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

            con_log = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + s + ")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = XE))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd_log = new OracleCommand("", con_log);
            con_log.Open();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader sr;
            string ip_base="localhost";
            try
            {
                if ((sr = new StreamReader(@"ip_base.txt")) != null)
                    ip_base = sr.ReadLine();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error Base!");

            }
            connect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cmd_log.CommandText = "SELECT pk_worker, status, FIO from worker where login ='" + textBox1.Text + "' and password = '" + textBox2.Text + "'";
            dr_log = cmd_log.ExecuteReader();
            dr_log.Read();
            //try
            //{
                string ss = dr_log[0].ToString();
                static_class.worker = Convert.ToInt32(ss);
                ss = dr_log[1].ToString();
                static_class.worker_status = Convert.ToInt32(ss);
                ss = dr_log[2].ToString();
                static_class.worker_fio = ss;
                Tovari tov = new Tovari();
                this.Visible = false;
                tov.ShowDialog();
                if (static_class.exit)
                {
                    this.Close();
                    con_log.Close();
                }
                else
                {
                    this.Visible = true;
                    this.textBox1.Focus();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    static_class.exit = true;
                }
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Неверный логин / пароль");
            //}
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch == 13)
            {
                cmd_log.CommandText = "SELECT pk_worker, status, FIO from worker where login ='" + textBox1.Text + "' and password = '" + textBox2.Text + "'";
                dr_log = cmd_log.ExecuteReader();
                dr_log.Read();
                //try
                //{
                string ss = dr_log[0].ToString();
                static_class.worker = Convert.ToInt32(ss);
                ss = dr_log[1].ToString();
                static_class.worker_status = Convert.ToInt32(ss);
                ss = dr_log[2].ToString();
                static_class.worker_fio = ss;
                Tovari tov = new Tovari();
                this.Visible = false;
                tov.ShowDialog();
                if (static_class.exit)
                {
                    this.Close();
                    con_log.Close();
                }
                else
                {
                    this.Visible = true;
                    this.textBox1.Focus();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    static_class.exit = true;
                }
            }
        }
    }
}
