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
    public partial class Akt_spis : Form
    {
        List<int> list_prod = new List<int>();
        int pk;
        OracleCommand cmd_spis;
        OracleConnection con_spis;
        OracleDataReader dr_spis;
        int rejim;
        string str1;

        public Akt_spis(int pkk)
        {
            pk = pkk;
            rejim = 1;
            InitializeComponent();
        }

        public Akt_spis(List<int> list)
        {
            rejim = 0;
            list_prod = list;
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

            con_spis = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + s + ")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = XE))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd_spis = new OracleCommand("", con_spis);
            con_spis.Open();
        }

        public void load_spis()
        {
            str1 = DateTime.Now.ToString();
            textBox3.Text = str1;
            cmd_spis.CommandText = "select worker.fio from worker where pk_worker = " + static_class.worker.ToString();
            dr_spis = cmd_spis.ExecuteReader();
            dr_spis.Read();
            textBox1.Text = dr_spis[0].ToString();

            cmd_spis.CommandText = "SELECT product.name, input_act.date_inp, status.name, product.MIN_INP_PRICE, product.PK_PROD FROM status, product, input_act where product.PK_act = input_act.PK_act and product.PK_stat = status.PK_stat";

            dr_spis = cmd_spis.ExecuteReader();

            dataGridView1.Rows.Clear();
            while (dr_spis.Read())
            {
                foreach (int element in list_prod)
                {
                    if (Convert.ToInt32(dr_spis[4].ToString()) == element)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dr_spis[0].ToString(); // naimenov
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = dr_spis[1].ToString(); //DATE
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = dr_spis[2].ToString(); // stat
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = dr_spis[3].ToString(); // min price
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = dr_spis[4].ToString(); // PK
                    }
                }
            }
        }

        public void load_view()
        {
            textBox2.Enabled = false;
            button2.Visible = false;
            string sss = "select act_spis.date_spis, act_spis.prichina, worker.fio from act_spis, worker where worker.pk_worker = act_spis.pk_worker and act_spis.pk_act_spis = " + pk.ToString();

            cmd_spis.CommandText = sss;
            dr_spis = cmd_spis.ExecuteReader();
            dr_spis.Read();
            textBox1.Text = dr_spis[0].ToString();
            textBox2.Text = dr_spis[1].ToString();
            textBox3.Text = dr_spis[2].ToString();

            sss = "SELECT product_ar.name, input_act.date_inp, status.name, product_ar.MIN_INP_PRICE, product_ar.PK_PROD_ar FROM status, product_ar, input_act where product_ar.PK_act = input_act.PK_act and product_ar.PK_stat = status.PK_stat and product_ar.pk_act_spis = " + pk.ToString();


            cmd_spis.CommandText = sss;
            dr_spis = cmd_spis.ExecuteReader();

            while (dr_spis.Read())
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dr_spis[0].ToString(); // naim
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = dr_spis[1].ToString(); // date
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = dr_spis[2].ToString(); //stat
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = dr_spis[3].ToString(); //min_price
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = dr_spis[4].ToString(); //pk
            }

        }

        private void Akt_spis_Load(object sender, EventArgs e)
        {
            connect();

            switch (rejim)
            {
                case 0: load_spis(); break;
                case 1: load_view(); break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
                MessageBox.Show("Поле Причина не заполнено", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else {

                string ss1 = "INSERT INTO act_spis (pk_worker, date_spis, prichina) VALUES ('" + static_class.worker.ToString() + "', to_date( '" + str1 + "','DD.MM.YYYY HH24:MI:SS' ), '" + textBox2.Text +"')";
                cmd_spis.CommandText = ss1;
                cmd_spis.ExecuteNonQuery();

                cmd_spis.CommandText = "select max(pk_act_spis) from act_spis";
                dr_spis = cmd_spis.ExecuteReader();
                dr_spis.Read();
                string temp = dr_spis[0].ToString();//pk_act


                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                            List<elemOfConfTable> list_elemConfTable = new List<elemOfConfTable>();

                            cmd_spis.CommandText = "SELECT pk_tab, value, pk_prod, pk_char from table_conform where pk_prod = " + dataGridView1.Rows[i].Cells[4].Value.ToString(); ;
                            dr_spis = cmd_spis.ExecuteReader();

                            while (dr_spis.Read())
                            {
                                list_elemConfTable.Add(new elemOfConfTable(Convert.ToInt32(dr_spis[0]), dr_spis[1].ToString(), Convert.ToInt32(dr_spis[2]), Convert.ToInt32(dr_spis[3])));
                            }

                            cmd_spis.CommandText = "insert into PRODUCT_AR (pk_prod_ar, NAME, SN, PK_SUBCAT, PK_CHEQUE, PK_ACT, MIN_INP_PRICE, FINISH_PRICE, EXPECT_PRICE, OPISANIE, FLAG_OWNER, COMISSION, PAY_STAY, PK_STAT, GARANT) select product.pk_prod, product.name, product.SN, product.PK_SUBCAT, product.PK_CHEQUE, product.PK_ACT, product.MIN_INP_PRICE, product.FINISH_PRICE, product.EXPECT_PRICE, product.OPISANIE, product.FLAG_OWNER, product.COMISSION, product.PAY_STAY, product.PK_STAT, product.GARANT  from product where pk_prod = " + dataGridView1.Rows[i].Cells[4].Value.ToString();
                            cmd_spis.ExecuteNonQuery();

                            
                            string sss = "UPDATE product_ar SET PK_ACT_SPIS = '" + temp + "' where pk_prod_ar = " + dataGridView1.Rows[i].Cells[4].Value.ToString();
                            cmd_spis.CommandText = sss;
                            cmd_spis.ExecuteNonQuery();

                            for (int j = 0; j < list_elemConfTable.Count; j++)
                            {
                                cmd_spis.CommandText = list_elemConfTable[j].makeSQLinsert_AR();
                                cmd_spis.ExecuteNonQuery();
                            }

                            string ss = "delete from table_conform where table_conform.pk_prod = " + dataGridView1.Rows[i].Cells[4].Value.ToString();

                            cmd_spis.CommandText = ss;// "delete from table_conform where table_conform.pk_prod = " + dataGridView1.Rows[i].Cells[9].Value.ToString();
                            cmd_spis.ExecuteNonQuery();

                            cmd_spis.CommandText = "delete from product where product.pk_prod = " + dataGridView1.Rows[i].Cells[4].Value.ToString();
                            cmd_spis.ExecuteNonQuery();
                }
                this.Close();
            }
        }
    }
}
