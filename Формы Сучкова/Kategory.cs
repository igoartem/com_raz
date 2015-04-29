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
    public partial class Kategory : Form
    {
        OracleCommand cmd_kat;  // нужно для подключения к базе данных 
        OracleConnection con_kat;
        OracleDataReader dr_kat;
        private Category me;
        private List<Category> list_cat;
        public Kategory()
        {
            InitializeComponent();

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // string name = dataGridView1.CurrentRow.Cells[1].ToString();
            int cur_pk = Convert.ToInt32(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            // me = new Category(name,pk);
            me = list_cat.Find(x => x.pk_cat == cur_pk);
            P_kategory p_kat = new P_kategory(me);
            p_kat.ShowDialog();
        }

        private void Kategory_Load(object sender, EventArgs e)
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
            con_kat = new OracleConnection(con_str); // подключение к бд с логином admin и паролем 123
            cmd_kat = new OracleCommand("", con_kat);
            con_kat.Open();
            cmd_kat.CommandText = "SELECT * from CATEGORY"; // запрос на получение всех данных о категориях
            dr_kat = cmd_kat.ExecuteReader();

            list_cat = new List<Category>();



            while (dr_kat.Read())
            {
                int pk = Convert.ToInt32(dr_kat[0]);
                string name = dr_kat[1].ToString();
                list_cat.Add(new Category(name, pk));

                dataGridView1.Rows.Add(name, pk);

            }
            //dataGridView1.DataBindings.Add(list_cat);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int cur_pk = Convert.ToInt32(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            string new_name = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            me = list_cat.Find(x => x.pk_cat == cur_pk);
            me.name = new_name;

            string ss = me.makeSQLupdate();

            cmd_kat.CommandText = ss;
            cmd_kat.ExecuteNonQuery();

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            me = new Category("Новая категория");
            string s = me.makeSQLinsert();
            cmd_kat.CommandText = s;
            cmd_kat.ExecuteNonQuery();

            s = "select max(pk_cat) from category";
            cmd_kat.CommandText = s;
            dr_kat = cmd_kat.ExecuteReader();
            dr_kat.Read();
            int pk_cat = Convert.ToInt32(dr_kat[0]);
            me.pk_cat=pk_cat;

            list_cat.Add(me);
            dataGridView1.Rows.Add(me.name, me.pk_cat);


        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {

                    if (MessageBox.Show("Вы действительно хотите удалить выбранные категории?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {

                        foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                        {
                            int cur_pk = Convert.ToInt32(row.Cells[1].Value.ToString());
                            me = list_cat.Find(x => x.pk_cat == cur_pk);
                            string s = me.makeSQLdelete();
                            cmd_kat.CommandText = s;
                            cmd_kat.ExecuteNonQuery();


                            list_cat.Remove(me);

                        }


                        dataGridView1.Rows.Clear();

                        foreach (Category c in list_cat)
                        {
                            dataGridView1.Rows.Add(c.name, c.pk_cat);


                        }
                    }
                }
                else            //если ничего не выделено, то удаляем текущую
                {
                    if (MessageBox.Show("Вы действительно хотите удалить данную категорию?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        int cur_pk = Convert.ToInt32(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                        me = list_cat.Find(x => x.pk_cat == cur_pk);
                        string s = me.makeSQLdelete();
                        cmd_kat.CommandText = s;
                        cmd_kat.ExecuteNonQuery();


                        list_cat.Remove(me);



                        dataGridView1.Rows.Clear();

                        foreach (Category c in list_cat)
                        {
                            dataGridView1.Rows.Add(c.name, c.pk_cat);


                        }

                    }



                } //MessageBox.Show("Сначала нужно выделить строки");
            }
            catch
            { 
                MessageBox.Show("Сначала нужно все соответствующие подкатегории"); 
            }
        }

        private void Kategory_FormClosed(object sender, FormClosedEventArgs e)
        {
            con_kat.Close();
        }
    }
}
