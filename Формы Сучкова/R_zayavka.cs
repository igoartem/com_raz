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
    public partial class R_zayavka : Form
    {
        private List<Request> list_zay;
        List<Category> list_category; // лист категорий
        List<Subcategory> list_subcategory; // лист подкатегорий
        Request me;
        int flag;   //0-add, 1-edit

        OracleCommand cmd_r_tovar;  // нужно для подключения к базе данных 
        OracleConnection con_r_tovar;
        OracleDataReader dr_r_tovar;

        public R_zayavka()
        {
            InitializeComponent();
        }

        public R_zayavka(List<Request> list_zay)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.list_zay = list_zay;
            flag = 0;
        }

        public R_zayavka(List<Request> list_zay, Request me)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.list_zay = list_zay;
            this.me = me;
            flag = 1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = -1;
            comboBox2.Items.Clear();

            string name = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            int pk = 0;

            int kol_vo = list_category.Count;
            for (int i = 0; i < kol_vo; i++)
            {
                if (list_category[i].name == name)
                    pk = list_category[i].pk_cat;

            }

            kol_vo = list_subcategory.Count;
            for (int i = 0; i < kol_vo; i++)
            {
                if (list_subcategory[i].pk_cat == pk)
                {
                    comboBox2.Items.Add(list_subcategory[i].name);
                }
            }
        }

        private void R_zayavka_Load(object sender, EventArgs e)
        {
            list_category = new List<Category>(); //создание листа с категориями
            list_subcategory = new List<Subcategory>(); // создание листа с подкатегориями


            StreamReader sr;
            string s = "localhost"; // подключение по умолчани.
            try
            {
                if ((sr = new StreamReader(@"ip_base.txt")) != null) //считывание ip - адреса базы с файла
                    s = sr.ReadLine();
            }

            catch (Exception)
            {
                MessageBox.Show("Error Base!");
            }
            con_r_tovar = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + s + ")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = xe))); User Id=" + "admin" + ";Password=" + "123" + ";"); // подключение к бд с логином admin и паролем 123
            cmd_r_tovar = new OracleCommand("", con_r_tovar);
            con_r_tovar.Open();
            cmd_r_tovar.CommandText = "SELECT * from CATEGORY"; // запрос на получение всех данных о категориях
            dr_r_tovar = cmd_r_tovar.ExecuteReader();

            while (dr_r_tovar.Read())
            {
                comboBox1.Items.Add(dr_r_tovar[1].ToString()); // заполение комбобокса для категорий
                //добавление данных в лист категорий
                list_category.Add(new Category(dr_r_tovar[1].ToString(), Convert.ToInt32(dr_r_tovar[0])));
            }

            cmd_r_tovar.CommandText = "select * from SUBCATEGORY"; // запрос на получение всех подкатегорий

            dr_r_tovar = cmd_r_tovar.ExecuteReader();
            // добавление всех подкатегорий в лист подкатегорий
            while (dr_r_tovar.Read())
            {
                list_subcategory.Add(new Subcategory(Convert.ToInt32(dr_r_tovar[0]), dr_r_tovar[2].ToString(), Convert.ToInt32(dr_r_tovar[1]), Convert.ToInt32(dr_r_tovar[3]), Convert.ToInt32(dr_r_tovar[4])));

            }

            if (flag == 1)
                load_old();
        }

        private void load_old()
        {
            textBoxAbout.Text = me.about;
            textBoxFIO.Text = me.FIO;
            textBoxPhone.Text = me.phone;
            textBoxPrice.Text = me.price.ToString();

            string ss = "select PK_CAT from SUBCATEGORY where PK_SUBCAT=" + me.pk_subcat;
            cmd_r_tovar.CommandText = ss;
            dr_r_tovar = cmd_r_tovar.ExecuteReader();
            dr_r_tovar.Read();

            int pk_cat = Convert.ToInt32(dr_r_tovar[0]);
            string name_cat = "", name_subcat = "";

            int kol_vo = list_category.Count;
            for (int i = 0; i < kol_vo; i++)

                if (list_category[i].pk_cat == pk_cat)
                    name_cat = list_category[i].name;


            for (int i = 0; i < comboBox1.Items.Count; i++)
                if (comboBox1.Items[i].ToString() == name_cat)
                    comboBox1.SelectedIndex = i;
            kol_vo = list_subcategory.Count;
            for (int i = 0; i < kol_vo; i++)
                if (list_subcategory[i].pk_subcat == me.pk_subcat)
                    name_subcat = list_subcategory[i].name;

            for (int i = 0; i < comboBox2.Items.Count; i++)
                if (comboBox2.Items[i].ToString() == name_subcat)
                    comboBox2.SelectedIndex = i;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (flag == 0)
            {

                get_all();      //возвращает готовый me

                if (me != null)
                {
                    list_zay.Add(me);
                    string s = me.makeSQLinsert();
                    cmd_r_tovar.CommandText = s;
                    cmd_r_tovar.ExecuteNonQuery();
                    this.Close();
                }
            }

            if (flag == 1)
            {
                int f = update_all();
                if (f == 0)
                {
                    string s = me.makeSQLupdate();
                    cmd_r_tovar.CommandText = s;
                    cmd_r_tovar.ExecuteNonQuery();
                    this.Close();
                }



            }





        }

        private int update_all()
        {
            if (textBoxAbout.Text == "")
            {
                MessageBox.Show("Не заполнено поле описание!");
                return 1;
            }
            else
                me.about = textBoxAbout.Text;

            if (textBoxFIO.Text == "")
            {
                MessageBox.Show("Не заполнено поле ФИО!");
                return 1;
            }
            else
                me.FIO = textBoxFIO.Text;

            if (textBoxPhone.Text == "")
            {
                MessageBox.Show("Не заполнено поле телефон!");
                return 1;
            }
            else
                me.phone = textBoxPhone.Text;

            if (textBoxPrice.Text == "")
            {
                MessageBox.Show("Не заполнено поле цена!");
                return 1;
            }
            else
                try
                {
                    me.price = Convert.ToInt32(textBoxPrice.Text);
                }
                catch { }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Не заполнено поле категория!");
                return 1;

            }
            

            int index = comboBox2.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Не заполнено поле подкатегория!");
                return 1;

            }
            else
            {
                Subcategory sq = list_subcategory.Find(s => s.name == comboBox2.Items[index].ToString());
                me.pk_subcat = sq.pk_subcat;
            }

                   
            return 0;
        }

        private void get_all()
        {
            Random rand = new Random();
            me = new Request(rand.Next(), "Новый заявитель", list_subcategory[0].pk_subcat, "", DateTime.Now, "", 0);




            if (textBoxAbout.Text == "")
            {
                MessageBox.Show("Не заполнено поле описание!");
                me = null;
                return;
            }
            else
                me.about = textBoxAbout.Text;

            if (textBoxFIO.Text == "")
            {
                MessageBox.Show("Не заполнено поле ФИО!");
                me = null;
                return;
            }
            else
                me.FIO = textBoxFIO.Text;

            if (textBoxPhone.Text == "")
            {
                MessageBox.Show("Не заполнено поле телефон!");
                me = null;
                return;
            }
            else
                me.phone = textBoxPhone.Text;

            if (textBoxPrice.Text == "")
            {
                MessageBox.Show("Не заполнено поле цена!");
                me = null;
                return;
            }
            else
                try
                {
                    me.price = Convert.ToInt32(textBoxPrice.Text);
                }
                catch { }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Не заполнено поле категория!");
                me = null;
                return;

            }
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Не заполнено поле подкатегория!");
                me = null;
                return;

            }
        }

        private void buttonCans_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
