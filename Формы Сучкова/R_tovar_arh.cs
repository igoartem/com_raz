﻿using System;
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
    public partial class R_tovar_arh : Form
    {
        OracleCommand cmd_r_tovar;  // нужно для подключения к базе данных 
        OracleConnection con_r_tovar;
        OracleDataReader dr_r_tovar;

        List<Category> list_category; // лист категорий
        List<Subcategory> list_subcategory; // лист подкатегорий
        public string name = "", serial_number = "", about_product = "";
        public int pk_akt_spis = 0, min_inp_price = 0, expected_price = 0, pk_subcat = 0, pay_stay = 0, flag_owner = 0;
        public bool flag_prod = false;
        public int pk_prod = 0;
        Product old_product; // объект типа Product, нужен для редактирования товара
        Product new_prod;

        public R_tovar_arh()
        {
            InitializeComponent();
        }

        public R_tovar_arh(Arhiv my,int pk) //конструктор для редактирования товаров
        {
            InitializeComponent();
            pk_prod = pk;
        }

        private void R_tovar_arh_Load(object sender, EventArgs e)
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
            load_product(pk_prod);
        }
        // метод на загрузку данных о товаре с БД и вывод его на форму
        public void load_product(int pk_product_ar)
        {
            string ss = "select * From Product_ar where PK_PROD_ar =" + pk_product_ar; // запрос на получение всей информации о товаре с ПК pk_product
            cmd_r_tovar.CommandText = ss;
            dr_r_tovar = cmd_r_tovar.ExecuteReader();
            dr_r_tovar.Read();
            int pk_check = 0;
            int fin_price = 0;
            if (dr_r_tovar[5].ToString() != "")
            {
                pk_check = Convert.ToInt32(dr_r_tovar[5]);
                fin_price = Convert.ToInt32(dr_r_tovar[8]);
            }
            else
                button3.Enabled = false;
            if (dr_r_tovar[1].ToString() != "")
            {
                pk_akt_spis = Convert.ToInt32(dr_r_tovar[1]);
            }
            else
                button4.Enabled = false;
            old_product = new Product(Convert.ToInt32(dr_r_tovar[0]), pk_akt_spis, dr_r_tovar[2].ToString(), dr_r_tovar[3].ToString(), Convert.ToInt32(dr_r_tovar[4]), Convert.ToInt32(dr_r_tovar[5]), Convert.ToInt32(dr_r_tovar[6]), Convert.ToInt32(dr_r_tovar[7]), Convert.ToInt32(dr_r_tovar[8]), Convert.ToInt32(dr_r_tovar[9]), dr_r_tovar[10].ToString());
            load_to_form();
        }

        public void load_to_form()
        {
            textBox1.Text = old_product.name;
            string ss = "select PK_CAT from SUBCATEGORY where PK_SUBCAT=" + old_product.pk_subcat;
            cmd_r_tovar.CommandText = ss;
            dr_r_tovar = cmd_r_tovar.ExecuteReader();
            dr_r_tovar.Read();

            int pk_cat = Convert.ToInt32(dr_r_tovar[0]);
            int kol_vo = list_category.Count;
            string name_cat = "", name_subcat = "";
            for (int i = 0; i < kol_vo; i++)

                if (list_category[i].pk_cat == pk_cat)
                    name_cat = list_category[i].name;


            for (int i = 0; i < comboBox1.Items.Count; i++)
                if (comboBox1.Items[i].ToString() == name_cat)
                    comboBox1.SelectedIndex = i;
            kol_vo = list_subcategory.Count;
            for (int i = 0; i < kol_vo; i++)
            {
                comboBox2.Items.Add(list_subcategory[i].name);
                if (list_subcategory[i].pk_subcat == old_product.pk_subcat)
                    name_subcat = list_subcategory[i].name;
            }
            for (int i = 0; i < comboBox2.Items.Count; i++)
                if (comboBox2.Items[i].ToString() == name_subcat)
                    comboBox2.SelectedIndex = i;

            textBox5.Text = old_product.sn;
            textBox6.Text = old_product.min_inp_price.ToString();
            textBox7.Text = old_product.expect_price.ToString();
            textBox8.Text = old_product.finish_price.ToString();
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            textBox4.Text = old_product.opisanie;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Prodaja check = new Prodaja(this, old_product.pk_cheque);
            check.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Akt_priem akt = new Akt_priem(this, old_product.pk_act);
            akt.ShowDialog();
        }
    }
}
