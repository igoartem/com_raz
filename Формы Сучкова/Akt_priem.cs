using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.OracleClient;
using System.IO;

namespace Формы_Сучкова
{
    public partial class Akt_priem : Form
    {
        List<Product> list_product;
        OracleCommand cmd_akt_priem;     //
        OracleConnection con_akt_priem;  // Подключение для вывода товаров в таблицу
        OracleDataReader dr_akt_priem;   //

        public Akt_priem()
        {
            InitializeComponent();
            list_product = new List<Product>();
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
            con_akt_priem = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + s + ")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = XE))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd_akt_priem = new OracleCommand("", con_akt_priem);
            con_akt_priem.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fio, number_phone,passport, seller;
            int kol_days;
            DateTime date_fin,date_start;

            if (textBox1.Text == "")
            {
                MessageBox.Show("Не заполнено поле ФИО!");
                return;
            }
            else
                fio = textBox1.Text;
            if (textBox2.Text=="")
            {
                MessageBox.Show("Не заполнено поле номер телефона!");
                return;
            }
            else
                number_phone = textBox2.Text;
            if (textBox3.Text == "")
            {
                MessageBox.Show("Не заполнено поле номер паспорта!");
                return;
            }
            else
                passport = textBox3.Text;
            if (textBox4.Text == "")
            {
                MessageBox.Show("Не заполнено поле номер количество дней!");
                return;
            }
            else
            {
                date_start = dateTimePicker1.Value;
                date_fin = dateTimePicker2.Value;
            }
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Не добавлено ни одного товара!");
                return;
            }

            

            string ss = "INSERT INTO SELLER (FIO, PASSPORT, PHONE) VALUES ('" + fio + "', '" + passport + "','" + number_phone + "')";
            cmd_akt_priem.CommandText = ss;
            cmd_akt_priem.ExecuteNonQuery();

            ss = "select pk_sell from seller where fio='"+ fio +"' and passport ='"+passport+"' and phone='"+number_phone+"'";
            cmd_akt_priem.CommandText = ss;
            dr_akt_priem = cmd_akt_priem.ExecuteReader();
            dr_akt_priem.Read();
            int pk_sell = Convert.ToInt32( dr_akt_priem[0]);

            Inpit_act inp_act = new Inpit_act(static_class.worker,pk_sell); //здесь надо будет менять работника

            inp_act.date_inp = date_start;
            inp_act.date_end = date_fin;
            ss = "INSERT INTO INPUT_ACT (DATE_INP, DATE_END, PK_SELL, PK_WORKER) VALUES (TO_DATE('"+inp_act.date_inp.ToString()+"','DD.MM.YYYY HH24:MI:SS'), TO_DATE('"+inp_act.date_end.ToString()+"','DD.MM.YYYY HH24:MI:SS'),'" + inp_act.pk_sell + "', '" + inp_act.pk_worker + "')";

            cmd_akt_priem.CommandText = ss;
            cmd_akt_priem.ExecuteNonQuery();

            ss = "SELECT PK_ACT From INPUT_ACT where PK_SELL='" + inp_act.pk_sell + "' AND pk_worker='" + inp_act.pk_worker + "' and date_INP=TO_DATE('" + inp_act.date_inp.ToString() + "','DD.MM.YYYY HH24:MI:SS') and date_end=TO_DATE('" + inp_act.date_end.ToString() + "','DD.MM.YYYY HH24:MI:SS')";
            cmd_akt_priem.CommandText = ss;
            dr_akt_priem = cmd_akt_priem.ExecuteReader();
            dr_akt_priem.Read();

            int pk_inp_act = Convert.ToInt32(dr_akt_priem[0]);


           // dr1 = cmd1.ExecuteReader();
            int kol_vo_tov = list_product.Count;
            for (int i = 0; i < kol_vo_tov; i++)
            {
                list_product[i].pk_act = pk_inp_act;
                ss=list_product[i].makeSQLinsert();
                cmd_akt_priem.CommandText = ss;
                cmd_akt_priem.ExecuteNonQuery();

            }

            this.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            R_tovar form_priem = new R_tovar(this);
            form_priem.ShowDialog();
            Product prod = static_class.product;
            list_product.Add(prod);
            add_datagrid(list_product[list_product.Count-1]);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
            {
                MessageBox.Show("Не выбран товар для удаления!");
                return;

            }
            else
            {
                list_product.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                update_datagrid();
            }
        }

        private void add_datagrid(Product prod)
        {
            dataGridView1.Rows.Add();
            int kol_vo_row = dataGridView1.RowCount - 1;

            if (list_product[list_product.Count - 1].flag_owner == 1)
                dataGridView1.Rows[kol_vo_row].Cells[0].Value = true;
            dataGridView1.Rows[kol_vo_row].Cells[1].Value = prod.name.ToString();
            dataGridView1.Rows[kol_vo_row].Cells[2].Value = prod.pay_stay;
            dataGridView1.Rows[kol_vo_row].Cells[3].Value = prod.min_inp_price;
            dataGridView1.Rows[kol_vo_row].Cells[4].Value = prod.expect_price;
            dataGridView1.Rows[kol_vo_row].Cells[5].Value = prod.comission;


        }

        private void update_datagrid(){

            dataGridView1.Rows.Clear();
            int kol_vo=list_product.Count;
            for (int i = 0; i < kol_vo;i++ )
            {
                add_datagrid(list_product[i]);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            int kol_days = Convert.ToInt32(textBox4.Text);
            DateTime dt = dateTimePicker1.Value;
            dt = dt.AddDays(kol_days);
            dateTimePicker2.Value = dt;
        }
    }
}
