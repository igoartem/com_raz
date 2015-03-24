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
        OracleCommand cmd_r_tovar;
        OracleConnection con_r_tovar;
        OracleDataReader dr_r_tovar;

        List<Category> list_category;
        List<Subcategory> list_subcategory;
        public string name = "", serial_number = "", about_product = "";
       public int commis = 0, min_inp_price = 0, expected_price = 0, pk_subcat = 0, pay_stay = 0, flag_owner = 0;
       public bool flag_prod = false;
        public int pk_prod=0;
         Product old_product;
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
        public R_tovar(Tovari my,int pk)
        {
            InitializeComponent();
            
           
            button5.Visible = false;
            button6.Visible = false;

          //  select * From Product where PK_PROD=
            //Product prod = new Product();
            flag_prod = true;
            pk_prod = pk;
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            get_all_old();


            //Product new_prod = new Product(pk_subcat, name, serial_number, min_inp_price, commis, pay_stay, expected_price, flag_owner);
            string ss;
            ss = old_product.makeSQLupdate();
            cmd_r_tovar.CommandText = ss;
            cmd_r_tovar.ExecuteNonQuery();
            this.Close();

        }

        private void R_tovar_Load(object sender, EventArgs e)
        {
            list_category = new List<Category>();
            list_subcategory = new List<Subcategory>();
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
            cmd_r_tovar = new OracleCommand("", con_r_tovar);
            con_r_tovar.Open();
            cmd_r_tovar.CommandText = "SELECT * from CATEGORY";
            dr_r_tovar = cmd_r_tovar.ExecuteReader();
            
            while (dr_r_tovar.Read())
            {
                comboBox1.Items.Add(dr_r_tovar[1].ToString());
                list_category.Add(new Category(dr_r_tovar[1].ToString(), Convert.ToInt32(dr_r_tovar[0])));
            }

            cmd_r_tovar.CommandText = "select * from SUBCATEGORY";

            dr_r_tovar = cmd_r_tovar.ExecuteReader();
            while (dr_r_tovar.Read())
            {
                list_subcategory.Add(new Subcategory(Convert.ToInt32(dr_r_tovar[0]),dr_r_tovar[2].ToString(), Convert.ToInt32(dr_r_tovar[1]), Convert.ToInt32(dr_r_tovar[3]), Convert.ToInt32(dr_r_tovar[4]) ));
                
            }
            if (flag_prod)
                load_product(pk_prod);

        }

        public void load_product(int pk_product)
        {
            string ss="select * From Product where PK_PROD="+pk_product;
            cmd_r_tovar.CommandText = ss;
            dr_r_tovar = cmd_r_tovar.ExecuteReader();
            dr_r_tovar.Read();
            int pk_check = 0;
            int fin_price=0;
            int garant = 0;
            if (dr_r_tovar[5].ToString() != "")
            {
                pk_check = Convert.ToInt32(dr_r_tovar[5]);
                fin_price = Convert.ToInt32(dr_r_tovar[11]);
                garant=Convert.ToInt32(dr_r_tovar[13]);
            }
            old_product = new Product(Convert.ToInt32(dr_r_tovar[0]), Convert.ToInt32(dr_r_tovar[1]), Convert.ToInt32(dr_r_tovar[2]), dr_r_tovar[3].ToString(), dr_r_tovar[4].ToString(), pk_check, Convert.ToInt32(dr_r_tovar[6]), Convert.ToInt32(dr_r_tovar[7]), Convert.ToInt32(dr_r_tovar[8]), Convert.ToInt32(dr_r_tovar[9]), Convert.ToInt32(dr_r_tovar[10]),fin_price , Convert.ToInt32(dr_r_tovar[12]), garant,dr_r_tovar[14].ToString());

            if (old_product.pk_cheque == 0)
                button3.Visible = false;
            load_to_form();
        }


        public void load_to_form()
        {
            textBox1.Text = old_product.name;
            string ss = "select PK_CAT from SUBCATEGORY where PK_SUBCAT=" + old_product.pk_subcat;
            cmd_r_tovar.CommandText = ss;
            dr_r_tovar = cmd_r_tovar.ExecuteReader();
            dr_r_tovar.Read();

            int pk_cat=Convert.ToInt32(dr_r_tovar[0]);
            int kol_vo = list_category.Count;
            string name_cat="",name_subcat="";
            for (int i = 0; i < kol_vo; i++)
            
                if (list_category[i].pk_cat == pk_cat)
                    name_cat = list_category[i].name;
            

            for (int i = 0; i < comboBox1.Items.Count; i++)
                if (comboBox1.Items[i].ToString() == name_cat)
                    comboBox1.SelectedIndex = i;
            kol_vo = list_subcategory.Count;
            for (int i = 0; i < kol_vo; i++)
                if (list_subcategory[i].pk_subcat == old_product.pk_subcat)
                    name_subcat = list_subcategory[i].name;

            for (int i = 0; i < comboBox2.Items.Count; i++)
                if (comboBox2.Items[i].ToString() == name_subcat)
                    comboBox2.SelectedIndex = i;

            textBox5.Text = old_product.sn;
            textBox2.Text = old_product.comission.ToString();
            textBox3.Text = old_product.pay_stay.ToString();
            textBox6.Text = old_product.min_inp_price.ToString();
            textBox7.Text = old_product.expect_price.ToString();
            if (old_product.pk_cheque != 0)
            {
                textBox8.Text = old_product.finish_price.ToString();
                label10.Visible = true;
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
                button1.Enabled = false;
            }
            else
            {
                textBox8.Visible = false;
                label10.Visible = false;
                button4.Visible = false;
            }
            textBox4.Text = old_product.opisanie;
            if (old_product.flag_owner == 1)
            checkBox1.Checked = true;

            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = -1;
            comboBox2.Items.Clear();
            
            string name = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            int pk=0;
         
            int kol_vo=list_category.Count;
            for (int i = 0; i < kol_vo; i++)
            {
                if (list_category[i].name == name)
                    pk = list_category[i].pk_cat;

            }

            kol_vo=list_subcategory.Count;
            for (int i = 0; i < kol_vo; i++)
            {
                if(list_subcategory[i].pk_cat==pk)
                {
                    comboBox2.Items.Add(list_subcategory[i].name);
                }
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


            get_all();

            
            Product new_prod = new Product(pk_subcat, name, serial_number, min_inp_price, commis, pay_stay, expected_price,flag_owner,about_product);
            static_class.product = new_prod;
            this.Close();

        }

        public void get_all()
        {
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
                commis = Convert.ToInt32(textBox2.Text);

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
                min_inp_price = Convert.ToInt32(textBox6.Text);

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
                about_product = textBox4.Text;

            if (checkBox1.Checked == true)
                flag_owner = 1;


            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Не заполнено поле категория!");
                return;

            }
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Не заполнено поле подкатегория!");
                return;

            }
            else
            {
                int kol_vo = list_subcategory.Count;
               
                for (int i = 0; i < kol_vo; i++)
                {
                    if (comboBox2.Items[comboBox2.SelectedIndex].ToString() == list_subcategory[i].name)
                        pk_subcat = list_subcategory[i].pk_subcat;

                }


            }


        }

        public void get_all_old()
        {
            get_all();
            old_product.name = name;
            old_product.pk_subcat = pk_subcat;
            old_product.sn = serial_number;
            old_product.min_inp_price = min_inp_price;
            old_product.comission = commis;
            old_product.pay_stay = pay_stay;
            old_product.expect_price = expected_price;
            old_product.flag_owner = flag_owner;
            old_product.opisanie = about_product;

        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex!=-1)
            for (int i = 0; i < list_subcategory.Count; i++)
                if (list_subcategory[i].name == comboBox2.Items[comboBox2.SelectedIndex])
                    textBox2.Text = list_subcategory[i].comission.ToString() ;
           
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void R_tovar_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Akt_priem akt = new Akt_priem(this,old_product.pk_act);
            akt.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Prodaja check = new Prodaja(this,old_product.pk_cheque);
            check.ShowDialog();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
