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

namespace Формы_Сучкова
{
    public partial class Tovari : Form
    {
        OracleCommand cmd1;
        OracleConnection con1;
        OracleDataReader dr1;
        bool load;
        int count = 0;

        public Tovari()
        {
            load = true;
            InitializeComponent();
        }

        public void refresh()
        {
            cmd1.CommandText = "SELECT product.name, category.name, subcategory.name, product.EXPECT_PRICE, status.NAME, product.FLAG_OWNER, input_act.DATE_INP, input_act.DATE_END, product.PK_PROD FROM input_act, product, status, category, subcategory where product.PK_SUBCAT = subcategory.PK_SUBCAT and subcategory.PK_CAT = category.PK_CAT and product.PK_STAT = status.PK_STAT and product.PK_ACT = input_act.PK_ACT ";

            dr1 = cmd1.ExecuteReader();

            int i = 0;

            dataGridView1.Enabled = false;
            while (dr1.Read())
            {
                dataGridView1.Rows.Add();
                for (int j = 0; j < 10; j++)
                {
                    if (j == 9)
                    {
                        //тут считаем колво дней остаток
                    }
                    else
                        if (j == 5)
                        {
                            if (Convert.ToInt32(dr1[j].ToString()) == 0)
                                dataGridView1.Rows[i].Cells[j + 1].Value = "Нет"; // статус товара наш не наш
                            else
                                dataGridView1.Rows[i].Cells[j + 1].Value = "Да";
                        }
                        else
                            dataGridView1.Rows[i].Cells[j + 1].Value = dr1[j].ToString();
                }


                i++;
            }
            count = dataGridView1.Rows.Count;
            dataGridView1.Enabled = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

            con1 = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = xe))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd1 = new OracleCommand("", con1);
            con1.Open();

            refresh();

            load = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Prodaja form_prodaja = new Prodaja();
            form_prodaja.Show();
        }

        private void редактированиеКатегорийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Kategory kategory = new Kategory();
            kategory.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Удаление
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "true")
                {
                    cmd1.CommandText = "delete from product where product.pk_prod = " + dataGridView1.Rows[i].Cells[0].Value.ToString();
                    refresh();
                }
            }
        }
    }
}
