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
    public partial class Tovari : Form
    {
        OracleCommand cmd1;     //
        OracleConnection con1;  // Подключение для вывода товаров в таблицу
        OracleDataReader dr1;   //

        OracleCommand cmd2;     //
        OracleConnection con2;  // Подключение для подсчета гарантии
        OracleDataReader dr2;   //

        List<Category> list_category;
        List<Subcategory> list_subcategory;

        bool load;              // флаг загрузилась ли форма
        int count = 0;          // число товаров
        TimeSpan end_days;      // оставшиеся дни гарантии

        public Tovari()
        {
            load = true;
            InitializeComponent();
        }

        public void find_refresh()
        {
            //Поиск
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox4.Items.Clear();

            list_subcategory.Clear();
            list_category.Clear();


            comboBox3.SelectedIndex = 0;
            
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
            cmd1.CommandText = "SELECT name from status";
            dr1 = cmd1.ExecuteReader();

            comboBox4.Items.Add("Неизвестно");
            while (dr1.Read())
            {
                comboBox4.Items.Add(dr1[0].ToString());
            }
            comboBox4.SelectedIndex = 0;
            if (comboBox2.Items.Count != 0)
                comboBox2.SelectedIndex = 0;
        }
        public void refresh()
        {
            refresh_telo();
            dataGridView1.Enabled = true;
        }

        public void refresh_telo()
        {
            cmd1.CommandText = "SELECT product.name, category.name, subcategory.name, product.EXPECT_PRICE, status.NAME, product.FLAG_OWNER, input_act.DATE_INP, input_act.DATE_END, product.PK_PROD FROM input_act, product, status, category, subcategory where product.PK_SUBCAT = subcategory.PK_SUBCAT and subcategory.PK_CAT = category.PK_CAT and product.PK_STAT = status.PK_STAT and product.PK_ACT = input_act.PK_ACT";
            dr1 = cmd1.ExecuteReader();

            int i = 0;

            dataGridView1.Enabled = false;      // начинаем загрузку и блокируем грид
            dataGridView1.Rows.Clear();
            while (dr1.Read())
            {
                dataGridView1.Rows.Add();
                for (int j = 0; j < 10; j++)
                {
                    if (j == 9)
                    {
                        //тут считаем колво оставшихся дней гарантии (через второе подключение)
                        if(dataGridView1.Rows[i].Cells[5].Value.ToString() == "Продано")
                        {
                            cmd2.CommandText = "SELECT cheque.date_ch, product.garant FROM cheque, product where cheque.pk_cheque = product.pk_cheque and product.pk_prod = '" + dataGridView1.Rows[i].Cells[j].Value.ToString() + "'";
                            dr2 = cmd2.ExecuteReader();
                            dr2.Read();
                            end_days = Convert.ToDateTime(dr2[0].ToString()).AddDays(Convert.ToInt32(dr2[1].ToString())) - DateTime.Now;
                            dataGridView1.Rows[i].Cells[j + 1].Value = end_days.Days;
                        }
                    }
                    else
                        if (j == 5) //тут нужно показать товар - наш или нет
                        {
                            if (Convert.ToInt32(dr1[j].ToString()) == 0)
                                dataGridView1.Rows[i].Cells[j + 1].Value = "Нет"; // статус товара наш не наш
                            else
                                dataGridView1.Rows[i].Cells[j + 1].Value = "Да";
                        }
                        else
                            dataGridView1.Rows[i].Cells[j + 1].Value = dr1[j].ToString(); //иначе просто выводим остальные данные в грид
                }


                i++;
            }
            count = dataGridView1.Rows.Count;
            //dataGridView1.Enabled = true;       // посчитали все элементы и включили грид
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void Tovari_Load(object sender, EventArgs e)
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

            con1 = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = "+ s +")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = XE))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd1 = new OracleCommand("", con1);
            con1.Open();

            con2 = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + s + ")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = XE))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd2 = new OracleCommand("", con2);
            con2.Open();

            refresh(); // обновим грид
            find_refresh();

            load = false; // и у же не загружаемся

            
        }

        private void button1_Click(object sender, EventArgs e) //открываем окно продажи
        {
            //передавать массив с пк продаваемых товаров

            List<int> list = new List<int>();

            for (int i = 0; i < dataGridView1.RowCount; i++)    //обходим грид и смотрим есть ли чекнутые товары
            {
                if (dataGridView1.Rows[i].Cells[0].Value != null)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "true")
                    {
                        list.Add(Convert.ToInt32(dataGridView1.Rows[i].Cells[9].Value.ToString()));
                    }
                }
            }
            if (list.Count != 0)
            {
                Prodaja form_prodaja = new Prodaja(list); // вызвали форму и передали лист с пк товаров
                form_prodaja.ShowDialog();
                refresh();
            }
            else
                MessageBox.Show("Не выбран ни один товар для продажи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void редактированиеКатегорийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Kategory kategory = new Kategory();
            kategory.ShowDialog();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //Удаление
            
            for (int i = 0; i < dataGridView1.RowCount; i++) //просто отправим в архив чекнутые товары
            {
                if (dataGridView1.Rows[i].Cells[0].Value != null)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "true")
                    {
                        if (dataGridView1.Rows[i].Cells[5].Value.ToString() != "Продано")
                        {
                            MessageBox.Show("Товар " + dataGridView1.Rows[i].Cells[1].Value.ToString() + " не продан!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        else if (dataGridView1.Rows[i].Cells[10].Value != null && (int)dataGridView1.Rows[i].Cells[10].Value > 0)
                        {
                            MessageBox.Show("У товара " + dataGridView1.Rows[i].Cells[1].Value.ToString() + " не кончилась гарантия!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        else
                        {

                            string sr="insert into PRODUCT_AR (pk_prod_ar, NAME, SN, PK_SUBCAT, PK_CHEQUE, PK_ACT, MIN_INP_PRICE, FINISH_PRICE, EXPECT_PRICE) select product.pk_prod, product.name, product.SN, product.PK_SUBCAT, product.PK_CHEQUE, product.PK_ACT, product.MIN_INP_PRICE, product.FINISH_PRICE, product.EXPECT_PRICE  from product where pk_prod = " + dataGridView1.Rows[i].Cells[9].Value.ToString();
                            cmd1.CommandText = sr;
                            cmd1.ExecuteNonQuery();
                            cmd1.CommandText = "delete from product where product.pk_prod = " + dataGridView1.Rows[i].Cells[9].Value.ToString();
                            cmd1.ExecuteNonQuery();
                        }
                    }
                }
            }
            
            refresh();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Akt_priem add_tovar = new Akt_priem();
            add_tovar.ShowDialog();
            refresh();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            for (int i = 0; i < list_subcategory.Count; i++ )
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

        private void comboBox1_RightToLeftChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            find_refresh();
            refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(dataGridView1.RowCount < count)
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
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value) < Convert.ToInt32(textBox1.Text))//мин цена 4
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
                if (textBox2.Text != "")
                {
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value) > Convert.ToInt32(textBox2.Text))//мин цена 4
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
                if (comboBox3.Text != "Неизвестно")
                {
                    if (dataGridView1.Rows[i].Cells[6].Value.ToString() != comboBox3.Text)//мин цена 4
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        i--;
                        continue;
                    };//выкуплен
                }
                if (comboBox4.Text != "Неизвестно")
                {
                    if (dataGridView1.Rows[i].Cells[5].Value.ToString() != comboBox4.Text)//мин цена 4
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        i--;
                        continue;
                    };//статус
                }
            }
            dataGridView1.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int cnt = 0, pk = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)    //обходим грид и смотрим есть ли чекнутые товары
            {
                if (dataGridView1.Rows[i].Cells[0].Value != null)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "true")
                    {
                        cnt++;
                        pk = Convert.ToInt32(dataGridView1.Rows[i].Cells[9].Value);
                    }
                }
            }
            if (cnt == 1)
            {
                R_tovar r_tovar = new R_tovar(this, pk); //вызов описания фии
                r_tovar.ShowDialog();
                refresh();
            }
            else
                MessageBox.Show("Выберите 1 товар", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
            int pk = Convert.ToInt32(dataGridView1.CurrentRow.Cells[9].Value);
            R_tovar r_tovar = new R_tovar(this, pk); //вызов описания фии
            r_tovar.ShowDialog();
            refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Arhiv arh = new Arhiv();
            arh.ShowDialog();
            refresh();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


    }
}
