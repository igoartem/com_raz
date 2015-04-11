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
    public partial class Prodaja : Form
    {
        List<int> list_prod = new List<int>();
        OracleCommand cmd_prod;
        OracleConnection con_prod;
        OracleDataReader dr_prod;

        bool loading = true;
        int summ = 0;
        int stock_garant = 0;
        int rejim = -1; // 0 - пришли продать 1 - пришли посмотреть чек
        int pk = 0;

        string str0;// PK
        string str1; // NAIMENOV
        string str2; // FIO
        string str3; // Garant
        string str4;  // EXp_cost
        string str5; // Fin_cost
        string temp;

        public void load_view()
        {
            cmd_prod.CommandText = "select date_ch, worker.fio from cheque, worker where PK_CHEQUE = " + pk.ToString() + " and cheque.PK_WORKER = worker.PK_WORKER";
            dr_prod = cmd_prod.ExecuteReader();
            dr_prod.Read();
            textBox5.Text = dr_prod[0].ToString();
            textBox4.Text = dr_prod[1].ToString();

            cmd_prod.CommandText = "select product.pk_prod, product.name, seller.FIO, product.garant, product.expect_price,product.finish_price,product.min_inp_price from product, input_act, seller where product.PK_cheque = " + pk.ToString() + " and product.PK_ACT = input_act.PK_ACT and input_act.PK_SELL = seller.PK_SELL";
            dr_prod = cmd_prod.ExecuteReader();

            while (dr_prod.Read())
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dr_prod[0].ToString(); // PK
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = dr_prod[1].ToString(); // naimenov
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = dr_prod[2].ToString(); //FIO
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = dr_prod[3].ToString(); //garant
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = dr_prod[4].ToString(); //exp_cost
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].Value = dr_prod[5].ToString(); // Fin_price
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[7].Value = dr_prod[6].ToString(); // min price
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].ReadOnly = true;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].ReadOnly = true;

            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = (Convert.ToInt32(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value) - Convert.ToInt32(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].Value));

            summ += Convert.ToInt32(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].Value);
            }
            textBox7.Text = summ.ToString();
        }

        public void load_prod()
        {
            textBox5.Text = DateTime.Now.ToString();

            StreamReader sr;
            string s = null;
            try
            {
                if ((sr = new StreamReader(@"stock_garant.txt")) != null)
                    stock_garant = Convert.ToInt32(sr.ReadLine());
            }

            catch (Exception)
            {
                MessageBox.Show("Error Base!");
            }

            cmd_prod.CommandText = "SELECT product.PK_PROD, product.name, seller.FIO, product.EXPECT_PRICE, product.FINISH_PRICE, product.pk_stat, product.min_inp_price FROM product, input_act, seller where product.PK_act = input_act.PK_act and input_act.pk_sell = seller.PK_SELL";

            dr_prod = cmd_prod.ExecuteReader();

            dataGridView1.Rows.Clear();
            while (dr_prod.Read())
            {
                foreach (int element in list_prod)
                {
                    if (Convert.ToInt32(dr_prod[0].ToString()) == element && Convert.ToInt32(dr_prod[5].ToString()) != static_class.tov_status) // 22 - ПК О СТАТУСЕ ПРОДАЖИ
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dr_prod[0].ToString(); // PK
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = dr_prod[1].ToString(); // naimenov
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = dr_prod[2].ToString(); //FIO
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = stock_garant; //stock_garant
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = dr_prod[3].ToString(); //exp_cost
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].Value = dr_prod[3].ToString(); // Fin_price
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[7].Value = dr_prod[6].ToString(); // min price

                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = (Convert.ToInt32(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value) - Convert.ToInt32(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].Value));

                        summ += Convert.ToInt32(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].Value);
                    }
                }
                textBox7.Text = summ.ToString();
            }
            loading = false;
            if (dataGridView1.RowCount == 0)
                button1.Enabled = false;
        }

        public Prodaja(List<int> list)
        {
            rejim = 0;
            loading = true;
            list_prod = list;
            InitializeComponent();
        }

        public Prodaja(R_tovar my,int pk_inp)
        {
            rejim = 1;
            pk = pk_inp;
            InitializeComponent();
            label5.Text = "Чек продажи";
            button1.Visible = false;
        }

        public Prodaja(R_tovar_arh my, int pk_inp)
        {
            rejim = 1;
            pk = pk_inp;
            InitializeComponent();
            label5.Text = "Чек продажи";
            button1.Visible = false;
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

            con_prod = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + s + ")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = XE))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd_prod = new OracleCommand("", con_prod);
            con_prod.Open();
        }

        private void Prodaja_Load(object sender, EventArgs e)
        {
            connect();
            switch (rejim)
            {
                case 0: load_prod(); break;
                case 1: load_view(); break;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            str0 = static_class.worker.ToString(); // РАБОТНИК ПО УМОЛЧАНИЮ
            str1 = DateTime.Now.ToString();
            string ss = "INSERT INTO cheque (pk_worker, date_ch) VALUES ('" + str0 + "', to_date( '" + str1 + "','DD.MM.YYYY HH24:MI:SS' ))";
            cmd_prod.CommandText = ss;
            cmd_prod.ExecuteNonQuery();

            cmd_prod.CommandText = "select max(pk_cheque) from CHEQUE";
            dr_prod = cmd_prod.ExecuteReader();
            dr_prod.Read();
            temp = dr_prod[0].ToString();//pk_cheque

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                str0 = dataGridView1.Rows[i].Cells[0].Value.ToString();    // PK
                str1 = dataGridView1.Rows[i].Cells[1].Value.ToString();    // NAIMENOV
                str2 = dataGridView1.Rows[i].Cells[2].Value.ToString();    // FIO
                str3 = dataGridView1.Rows[i].Cells[3].Value.ToString();    // Garant
                str4 = dataGridView1.Rows[i].Cells[4].Value.ToString();    // EXp_cost
                str5 = dataGridView1.Rows[i].Cells[6].Value.ToString();    // Fin_cost

                cmd_prod.CommandText = "UPDATE product set garant = '" + str3 + "', finish_price = '" + str5 + "', pk_stat = '" + 22 + "', pk_cheque = '" + temp + "' where pk_prod = '" + str0 + "'";
                cmd_prod.ExecuteNonQuery();
            }
            this.Close();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!loading)
            {
                if (Convert.ToInt32(dataGridView1.CurrentRow.Cells[6].Value) < Convert.ToInt32(dataGridView1.CurrentRow.Cells[7].Value))
                {
                    dataGridView1.CurrentRow.Cells[6].Value = dataGridView1.CurrentRow.Cells[7].Value;
                }
                if (dataGridView1.CurrentRow.Cells[6].Value != null && dataGridView1.CurrentRow.Cells[4].Value != null)
                {
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = (Convert.ToInt32(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value) - Convert.ToInt32(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].Value));
                }
                summ = 0;
                for(int i = 0; i< dataGridView1.RowCount; i++)
                {
                    summ += Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value);
                }
                textBox7.Text = summ.ToString();
            }
        }
    }
}
