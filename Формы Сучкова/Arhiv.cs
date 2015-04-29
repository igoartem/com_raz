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
    public partial class Arhiv : Form
    {
        OracleCommand cmd1;     //
        OracleConnection con1;  // Подключение для вывода товаров в таблицу
        OracleDataReader dr1;   //


        List<Category> list_category;
        List<Subcategory> list_subcategory;

        bool load;              // флаг загрузилась ли форма
        int count = 0;          // число товаров

        public Arhiv()
        {
            load = true;
            InitializeComponent();
        }

        private void prava()
        {
            if (static_class.worker_status == 0)
            {
                button6.Enabled = false;

            }

        }

        private void Arhiv_Load(object sender, EventArgs e)
        {
            load = true;

            list_category = new List<Category>();
            list_subcategory = new List<Subcategory>();

            StreamReader sr;        // загрузка файла с адресом хоста с бд
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

            con1 = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + s + ")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = XE))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd1 = new OracleCommand("", con1);
            con1.Open();

            refresh(); // обновим грид
            find_refresh();

            load = false; // и у же не загружаемся
            prava();
        }

        public void find_refresh()
        {
            //Поиск
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            list_subcategory.Clear();
            list_category.Clear();

            cmd1.CommandText = "SELECT * from CATEGORY";
            dr1 = cmd1.ExecuteReader();

            while (dr1.Read())
            {
                list_category.Add(new Category(dr1[1].ToString(), Convert.ToInt32(dr1[0])));
                comboBox1.Items.Add(list_category[list_category.Count - 1].name.ToString());
            }
            if (comboBox1.Items.Count != 0)
                comboBox1.SelectedIndex = 0;

            cmd1.CommandText = "SELECT * from SUBCATEGORY";
            dr1 = cmd1.ExecuteReader();

            while (dr1.Read())
            {
                list_subcategory.Add(new Subcategory(Convert.ToInt32(dr1[0]), dr1[2].ToString(), Convert.ToInt32(dr1[1]), Convert.ToInt32(dr1[3]), Convert.ToInt32(dr1[4])));
                if (list_subcategory[list_subcategory.Count - 1].pk_cat == list_category[0].pk_cat)
                    comboBox2.Items.Add(list_subcategory[list_subcategory.Count - 1].name.ToString());
            }
        }

        public void refresh()
        {
            refresh_telo();
            dataGridView1.Enabled = true;
        }

        public void refresh_telo()
        {
            cmd1.CommandText = "SELECT product_ar.name, category.name, subcategory.name, input_act.DATE_INP, product_ar.MIN_INP_PRICE, product_ar.EXPECT_PRICE, product_ar.FINISH_PRICE, product_ar.PK_PROD_ar FROM input_act, product_ar, category, subcategory where product_ar.PK_SUBCAT = subcategory.PK_SUBCAT and subcategory.PK_CAT = category.PK_CAT and product_ar.PK_ACT = input_act.PK_ACT";
            dr1 = cmd1.ExecuteReader();

            int i = 0;

            dataGridView1.Enabled = false;      // начинаем загрузку и блокируем грид
            dataGridView1.Rows.Clear();
            while (dr1.Read())
            {
                dataGridView1.Rows.Add();
                for (int j = 0; j < 8; j++)
                {
                    dataGridView1.Rows[i].Cells[j+1].Value = dr1[j].ToString(); //просто выводим остальные данные в грид
                }


                i++;
            }
            count = dataGridView1.Rows.Count;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++) //просто удалим чекнутые товары
            {
                if (dataGridView1.Rows[i].Cells[0].Value != null)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "true")
                    {
                        //textBox4.Text = dataGridView1.Rows[i].Cells[8].Value.ToString();
                        string ss = "delete from table_conform_ar where table_conform_ar.pk_prod_ar = " + dataGridView1.Rows[i].Cells[8].Value.ToString();

                        cmd1.CommandText = ss;// "delete from table_conform where table_conform.pk_prod = " + dataGridView1.Rows[i].Cells[9].Value.ToString();
                        cmd1.ExecuteNonQuery();
                        cmd1.CommandText = "delete from product_ar where product_ar.pk_prod_ar = " + dataGridView1.Rows[i].Cells[8].Value.ToString();
                        cmd1.ExecuteNonQuery();
                        //textBox4.Text = dataGridView1.Rows[i].Cells[9].Value.ToString();
                    }
                }
            }
            refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount < count)
                refresh_telo();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (textBox3.Text != "")
                {
                    if (dataGridView1.Rows[i].Cells[1].Value.ToString().IndexOf(textBox3.Text) < 0) //ищем по имени
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
                if (textBox1.Text != "")
                {
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value) < Convert.ToInt32(textBox1.Text))//мин цена 4
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
                if (textBox2.Text != "")
                {
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value) > Convert.ToInt32(textBox2.Text))//мин цена 4
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        i--;
                        continue;
                    }//макс цена
                }
                if (comboBox2.Text != "")
                {
                    if (dataGridView1.Rows[i].Cells[3].Value.ToString() != comboBox2.Text)//мин цена 4
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        i--;
                        continue;
                    };//подкат
                }
            }
            dataGridView1.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            for (int i = 0; i < list_subcategory.Count; i++)
            {
                if (list_subcategory[i].pk_cat == list_category[comboBox1.SelectedIndex].pk_cat)
                    comboBox2.Items.Add(list_subcategory[i].name.ToString());
            }
            if (comboBox2.Items.Count != 0)
                comboBox2.SelectedIndex = 0;
            else
            {
                comboBox2.Text = "";
                comboBox2.Items.Clear();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            find_refresh();
            refresh();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int pk = Convert.ToInt32(dataGridView1.CurrentRow.Cells[8].Value);
            R_tovar_arh r_tovar = new R_tovar_arh(this, pk); //вызов описания фии

            r_tovar.ShowDialog();
            refresh();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int pk = Convert.ToInt32(dataGridView1.CurrentRow.Cells[8].Value);
            R_tovar_arh r_tovar = new R_tovar_arh(this, pk); //вызов описания фии
            r_tovar.ShowDialog();
            refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Удаление

            for (int i = 0; i < dataGridView1.RowCount; i++) //просто отправим в архив чекнутые товары
            {
                if (dataGridView1.Rows[i].Cells[0].Value != null)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "true")
                    {
                        
                            List<elemOfConfTable> list_elemConfTable = new List<elemOfConfTable>();

                            cmd1.CommandText = "SELECT pk_tab_ar, value, pk_prod_ar, pk_char from table_conform_ar where pk_prod_ar = " + dataGridView1.Rows[i].Cells[8].Value.ToString(); ;
                            dr1 = cmd1.ExecuteReader();

                            while (dr1.Read())
                            {
                                list_elemConfTable.Add(new elemOfConfTable(Convert.ToInt32(dr1[0]), dr1[1].ToString(), Convert.ToInt32(dr1[2]), Convert.ToInt32(dr1[3])));
                            }

                            cmd1.CommandText = "insert into PRODUCT (pk_prod, NAME, SN, PK_SUBCAT, PK_CHEQUE, PK_ACT, MIN_INP_PRICE, FINISH_PRICE, EXPECT_PRICE, OPISANIE, FLAG_OWNER, COMISSION, PAY_STAY, PK_STAT, GARANT) select product_ar.pk_prod_ar, product_ar.name, product_ar.SN, product_ar.PK_SUBCAT, product_ar.PK_CHEQUE, product_ar.PK_ACT, product_ar.MIN_INP_PRICE, product_ar.FINISH_PRICE, product_ar.EXPECT_PRICE, product_ar.OPISANIE, product_ar.FLAG_OWNER, product_ar.COMISSION, product_ar.PAY_STAY, product_ar.PK_STAT, product_ar.GARANT  from product_ar where pk_prod_ar = " + dataGridView1.Rows[i].Cells[8].Value.ToString();
                            cmd1.ExecuteNonQuery();

                            for (int j = 0; j < list_elemConfTable.Count; j++)
                            {
                                cmd1.CommandText = list_elemConfTable[j].makeSQLinsert_return_from_AR();
                                cmd1.ExecuteNonQuery();
                            }

                            string ss = "delete from table_conform_ar where table_conform_ar.pk_prod_ar = " + dataGridView1.Rows[i].Cells[8].Value.ToString();

                            cmd1.CommandText = ss;// "delete from table_conform where table_conform.pk_prod = " + dataGridView1.Rows[i].Cells[9].Value.ToString();
                            cmd1.ExecuteNonQuery();

                            cmd1.CommandText = "delete from product_ar where product_ar.pk_prod_ar = " + dataGridView1.Rows[i].Cells[8].Value.ToString();
                            cmd1.ExecuteNonQuery();
                        
                    }
                }
            }

            refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Arhiv_FormClosed(object sender, FormClosedEventArgs e)
        {
            con1.Close();
    
        }

    }
}
