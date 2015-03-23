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
    public partial class Form1 : Form
    {
        OracleCommand cmd1;
        OracleConnection con1;
        OracleDataReader dr1;

        public Form1()
        {
            InitializeComponent();
            con1 = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.1.3)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = XE))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd1 = new OracleCommand("", con1);
            con1.Open();
            con1.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader sr;
            string ip_base="localhost";
            try
            {
                if ((sr = new StreamReader(@"ip_base.txt")) != null)
                    ip_base = sr.ReadLine();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error Base!");

            }
                


            con1 = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = "+ip_base+")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = xe))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd1 = new OracleCommand("", con1);
            con1.Open();

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
//примеры
//    private void Buyers_Load(object sender, EventArgs e)
//{
//    load = true;

//    con1 = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = xe))); User Id=" + "ADMIN" + ";Password=" + "td-w8901g" + ";");
//    cmd1 = new OracleCommand("", con1);
//    con1.Open();
//    // TODO: данная строка кода позволяет загрузить данные в таблицу "dataSet1.ZAK_VED_1". При необходимости она может быть перемещена или удалена.

//    cmd1.CommandText = "SELECT buyer.name, buyer.pk_buyer FROM buyer";

//    dr1 = cmd1.ExecuteReader();
//    int i = 0;

//    dataGridView1.Enabled = false;
//    while (dr1.Read())
//    {
//        dataGridView1.Rows.Add();
//        for (int j = 0; j < 2; j++)
//        {
//            dataGridView1.Rows[i].Cells[j].Value = dr1[j].ToString();
//        }
//        i++;
//    }
//    count = dataGridView1.Rows.Count;
//    dataGridView1.Enabled = true;

//    load = false;

//}

//private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
//{
//    string str0, str1, str2, str3;
//    if (load != true)
//    {
//        if (dataGridView1.RowCount != count)
//        {
//            bool NotNull = true;
//            for (int i = 0; i < 1; i++)
//                if (dataGridView1.Rows[e.RowIndex].Cells[i].Value == null)
//                    NotNull = false;

//            if (NotNull && dataGridView1.RowCount != count)
//            {
//                str0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();    // name

//                cmd1.CommandText = "INSERT INTO buyer (name) VALUES ('" + str0 + "')";
//                cmd1.ExecuteNonQuery();

//                count = dataGridView1.RowCount;

//                cmd1.CommandText = "SELECT pk_buyer from buyer where name = '" + str0 + "'";
//                dr1 = cmd1.ExecuteReader();
//                dr1.Read();
//                dataGridView1.Rows[e.RowIndex].Cells[1].Value = dr1[0].ToString();
//                count++;
//            }
//        }
//        else
//        {
//            bool NotNull = true;
//            for (int i = 0; i < 2; i++)
//                if (dataGridView1.Rows[e.RowIndex].Cells[i].Value == null)
//                    NotNull = false;

//            if (NotNull)
//            {
//                str0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();    // name
//                str1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();    // PK

//                cmd1.CommandText = "UPDATE buyer set name = '" + str0 + "' where pk_buyer = '" + str1 + "'";
//                cmd1.ExecuteNonQuery();
//            }
//        }
//    }
//}

//private void удалитьВыбранныйТоварToolStripMenuItem_Click(object sender, EventArgs e)
//{
//    if (count > 1)
//    {
//        string str3;
//        str3 = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString();

//        cmd1.CommandText = "SELECT * FROM check_1 WHERE check_1.PK_buyer = " + str3;
//        dr1 = cmd1.ExecuteReader();

//        if (!dr1.Read())
//        {
//            cmd1.CommandText = "delete buyer where PK_buyer = " + str3;
//            cmd1.ExecuteNonQuery();
//            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
//            count = dataGridView1.Rows.Count;
//        }
//        else
//            MessageBox.Show("Необходимо удалить все связанные записи!", "Удаление данной ведомости невозможно!");

//    }
//}
