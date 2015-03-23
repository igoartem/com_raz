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
        OracleCommand cmd_r_tovar,cmd_kat;
        OracleConnection con_r_tovar;
        OracleDataReader dr_r_tovar, dr_kat,dr_subkat;
        

        public R_tovar()
        {
            InitializeComponent();
        }
        public R_tovar(Akt_priem my)
        {
            InitializeComponent();
            label10.Visible = false;
            textBox8.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible=false;
            button1.Visible = false;
        }
        public R_tovar(Tovari my)
        {
            InitializeComponent();
            
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            


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
            con_r_tovar = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + s + ")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = xe))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd_kat = new OracleCommand("", con_r_tovar);
            con_r_tovar.Open();
            cmd_kat.CommandText = "SELECT * from CATEGORY";
            dr_kat = cmd_kat.ExecuteReader();
            while (dr_kat.Read())
            {
                comboBox1.Items.Add(dr_kat[1].ToString());
            }

            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = -1;
            comboBox2.Items.Clear();
            
            string name = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            int pk=0;
          //  cmd_r_tovar.CommandText = "SELECT * from CATEGORY";
            dr_kat = cmd_kat.ExecuteReader();
            
            while (dr_kat.Read())
            {
                if (dr_kat[1].ToString() == name)
                    pk = Convert.ToInt32(dr_kat[0]);
            }

            cmd_r_tovar.CommandText = "select * from SUBCATEGORY";

            dr_r_tovar = cmd_r_tovar.ExecuteReader();
            while (dr_r_tovar.Read())
            {
                if (Convert.ToInt32(dr_r_tovar[1]) == pk)
                    comboBox2.Items.Add(dr_r_tovar[2].ToString());
            }

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            
            
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (not_simvol(e))
            {
                e.Handled = true;
                return;
            }
            if (not_more(e,textBox2.Text.ToString()))
            {
                textBox2.Clear();
                textBox2.Text = "100";
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (not_simvol(e))
                e.Handled = true;
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (not_simvol(e))
                e.Handled = true;
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(not_simvol(e))
                e.Handled = true;
            
        }

        private bool not_simvol(KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8) //Если символ, введенный с клавы - не цифра (IsDigit),
            {
                return true;
                // то событие не обрабатывается. ch!=8 (8 - это Backspace)
            }
           
            return false;

        }
        private bool not_more(KeyPressEventArgs e,string str)
        {
            char ch = e.KeyChar;

            if (ch != 8 && Convert.ToInt32(str + ch) > 100)
            {
                return true;
            }
            return false;

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string name, serial_number, about_product;
            int komnis, base_price, expected_price,pk_subkat,pay_stay;

            if (textBox1.Text == "")
            {
                MessageBox.Show("Не заполнено поле название!");
                return;
            }
            else
                name = textBox1.Text;

            if (textBox5.Text == "")
            {
                MessageBox.Show("Не заполнено поле серийный номер!");
                return;
            }
            else
                serial_number = textBox5.Text;

            if (textBox2.Text == "")
            {
                MessageBox.Show("Не заполнено поле комиссия!");
                return;
            }
            else
                komnis = Convert.ToInt32(textBox2.Text);

            if (textBox3.Text == "")
            {
                MessageBox.Show("Не заполнено поле плата за простой!");
                return;
            }
            else
                pay_stay = Convert.ToInt32(textBox3.Text);

            if (textBox6.Text == "")
            {
                MessageBox.Show("Не заполнено поле базовая цена!");
                return;
            }
            else
                base_price = Convert.ToInt32(textBox6.Text);

            if (textBox7.Text == "")
            {
                MessageBox.Show("Не заполнено поле ожидаемая цена!");
                return;
            }
            else
                expected_price = Convert.ToInt32(textBox7.Text);

            if (textBox4.Text == "")
            {
                MessageBox.Show("Не заполнено поле описание товара!");
                return;
            }
            else
                about_product = textBox6.Text;

            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string name = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            //int pk = 0;
            //cmd_r_tovar.CommandText = "SELECT * from CATEGORY";
            //dr_r_tovar = cmd_r_tovar.ExecuteReader();
            //while (dr_r_tovar.Read())
            //{
            //    if (dr_r_tovar[1].ToString() == name)
            //        pk = Convert.ToInt32(dr_r_tovar[0]);
            //}

            //cmd_r_tovar.CommandText = "select * from SUBCATEGORY";

            //dr_r_tovar = cmd_r_tovar.ExecuteReader();
            //while (dr_r_tovar.Read())
            //{
            //    if (Convert.ToInt32(dr_r_tovar[1]) == pk)
            //        comboBox2.Items.Add(dr_r_tovar[2].ToString());
            //}
        }
    }
}
