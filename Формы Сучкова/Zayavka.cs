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
    public partial class Zayavka : Form
    {

        OracleCommand cmd_zay;  // нужно для подключения к базе данных 
        OracleConnection con_zay;
        OracleDataReader dr_zay;
        private Request me;
        private List<Request> list_zay;


        public Zayavka()
        {
            InitializeComponent();
        }

        private void Zayavka_Load(object sender, EventArgs e)
        {
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

            string con_str = "Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + s + ")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = xe))); User Id= admin; Password = 123;";
            static_connect.con_str = con_str;
            con_zay = new OracleConnection(con_str); // подключение к бд с логином admin и паролем 123
            cmd_zay = new OracleCommand("", con_zay);
            con_zay.Open();
            cmd_zay.CommandText = "SELECT * from REQUEST"; // запрос на получение всех 
            dr_zay = cmd_zay.ExecuteReader();

            list_zay = new List<Request>();



            while (dr_zay.Read())
            {
                int pk = Convert.ToInt32(dr_zay[0]);
                DateTime dateQ = DateTime.Now;
               // int pk_appl = Convert.ToInt32(dr_zay[2]);
                int pk_subcat = Convert.ToInt32(dr_zay[2]);
                string about = dr_zay[3].ToString();
                int price = Convert.ToInt32(dr_zay[4]);
                string FIO = dr_zay[5].ToString();
                string phone = dr_zay[6].ToString();
                
                list_zay.Add(new Request(pk, FIO, pk_subcat,about,dateQ,phone,price));


                //OracleCommand cmd_zay2 = new OracleCommand("", con_zay);
                cmd_zay.CommandText = "SELECT name from Subcategory where pk_subcat = " + pk_subcat;
                OracleDataReader dr_zay2 = cmd_zay.ExecuteReader();
                dr_zay2.Read();
                string subcat = dr_zay2[0].ToString();
                dataGridView1.Rows.Add(subcat, about, price, FIO, phone, pk);

            }
          
        }

        void ReloadDataGrid()
        {
            dataGridView1.Rows.Clear();

            foreach (Request c in list_zay)
            {

                cmd_zay.CommandText = "SELECT name from Subcategory where pk_subcat = " + c.pk_subcat;
                OracleDataReader dr_zay2 = cmd_zay.ExecuteReader();
                dr_zay2.Read();
                string subcat = dr_zay2[0].ToString();
                dataGridView1.Rows.Add(subcat, c.about, c.price, c.FIO, c.phone, c.pk_request);

            }
        
        
        
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {

                    if (MessageBox.Show("Вы действительно хотите удалить выбранные заявки?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {

                        foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                        {
                            int cur_pk = Convert.ToInt32(row.Cells[5].Value.ToString());
                            me = list_zay.Find(x => x.pk_request == cur_pk);
                            string s = me.makeSQLdelete();
                            cmd_zay.CommandText = s;
                            cmd_zay.ExecuteNonQuery();

                            list_zay.Remove(me);

                        }
                        ReloadDataGrid();                       
                    }
                }
                else            //если ничего не выделено, то удаляем текущую
                {
                    if (MessageBox.Show("Вы действительно хотите удалить данную заявку?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        int cur_pk = Convert.ToInt32(dataGridView1.CurrentRow.Cells[5].Value.ToString());
                        me = list_zay.Find(x => x.pk_request == cur_pk);
                        string s = me.makeSQLdelete();
                        cmd_zay.CommandText = s;
                        cmd_zay.ExecuteNonQuery();

                        list_zay.Remove(me);

                        ReloadDataGrid();
                    }
                } //MessageBox.Show("Сначала нужно выделить строки");
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            R_zayavka zay = new R_zayavka(list_zay);
            zay.ShowDialog();

            ReloadDataGrid();

            //me = new Request("Новый заявитель",0,"",DateTime.Now,"",0);
            //string s = me.makeSQLinsert();
            //cmd_zay.CommandText = s;
            //cmd_zay.ExecuteNonQuery();

            //s = "select max(pk_request) from request";
            //cmd_zay.CommandText = s;
            //dr_zay = cmd_zay.ExecuteReader();
            //dr_zay.Read();
            //int pk_request = Convert.ToInt32(dr_zay[0]);
            //me.pk_request = pk_request;

            //list_zay.Add(me);

            //cmd_zay.CommandText = "SELECT name from Subcategory";
            //OracleDataReader dr_zay2 = cmd_zay.ExecuteReader();
            //dr_zay2.Read();
            //string subcat = dr_zay2[0].ToString();
            //dataGridView1.Rows.Add(subcat, me.about, me.price, me.FIO, me.phone, me.pk_request);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int cur_pk = Convert.ToInt32(dataGridView1.CurrentRow.Cells[5].Value.ToString());
            // me = new Category(name,pk);
            me = list_zay.Find(x => x.pk_request == cur_pk);
            R_zayavka zay = new R_zayavka(list_zay, me);
            zay.ShowDialog();

            ReloadDataGrid();
        }

        private void Zayavka_FormClosed(object sender, FormClosedEventArgs e)
        {
            con_zay.Close();
        }
    }
}
