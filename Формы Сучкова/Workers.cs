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
    public partial class Workers : Form
    {
        OracleCommand cmd_w;     //
        OracleConnection con_w;  // Подключение для вывода товаров в таблицу
        OracleDataReader dr_w;   //

        public Workers()
        {
            InitializeComponent();
        }

        private void Workers_Load(object sender, EventArgs e)
        {
            connect();
            refresh();
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

            con_w = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + s + ")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = XE))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd_w = new OracleCommand("", con_w);
            con_w.Open();
        }

        public void refresh()
        {
            dataGridView1.Rows.Clear();
            cmd_w.CommandText = "SELECT pk_worker, fio, status, flag from worker";
            dr_w = cmd_w.ExecuteReader();

            while (dr_w.Read())
            {
                if (Convert.ToInt32(dr_w[3].ToString()) > 0)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dr_w[0].ToString(); // PK
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = dr_w[1].ToString(); // FIO
                    if( Convert.ToInt32(dr_w[2].ToString()) == 0)
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = "Работник"; // STATUS
                    else
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = "Руководитель";
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = dr_w[3].ToString(); // Flag
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add_redact_worker work_add = new Add_redact_worker(0,0);
            work_add.ShowDialog();
            refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int w_pk = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            Add_redact_worker work_add = new Add_redact_worker(1, w_pk);
            work_add.ShowDialog();
            refresh();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int w_pk = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            Add_redact_worker work_add = new Add_redact_worker(1, w_pk);
            work_add.ShowDialog();
            refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sss = "UPDATE worker SET flag = '" + 0 + "' where pk_worker = " + dataGridView1.CurrentRow.Cells[0].Value;
            cmd_w.CommandText = sss;
            cmd_w.ExecuteNonQuery();
            refresh();
        }
    }
}
