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
    public partial class P_kategory : Form
    {
        private Category kat_father;
        private List<Subcategory> list_pcat;
        private Subcategory me;


        OracleCommand cmd_pkat;  // нужно для подключения к базе данных 
        OracleConnection con_pkat;
        OracleDataReader dr_pkat;

        //public P_kategory()
        //{
        //    InitializeComponent();
        //}

        public P_kategory(Category kategory)
        {
            InitializeComponent();
            this.kat_father = kategory;
            textBoxKategory.Text = kat_father.name;
        }

        private void P_kategory_Load(object sender, EventArgs e)
        {
            StreamReader sr;
            con_pkat = new OracleConnection(static_connect.con_str); // подключение к бд с логином admin и паролем 123
            cmd_pkat = new OracleCommand("", con_pkat);
            con_pkat.Open();

            cmd_pkat.CommandText = "SELECT * from SUBCATEGORY WHERE PK_CAT=" + kat_father.pk_cat; // запрос на получение всех данных о категориях
            dr_pkat = cmd_pkat.ExecuteReader();

            list_pcat = new List<Subcategory>();

            dataGridView1.Rows.Clear();

            while (dr_pkat.Read())
            {
                int pk = Convert.ToInt32(dr_pkat[0]);
                int pk_cat = Convert.ToInt32(dr_pkat[1]);
                string name = dr_pkat[2].ToString();
                int com = Convert.ToInt32(dr_pkat[3]);
                int pay = Convert.ToInt32(dr_pkat[4]);
                list_pcat.Add(new Subcategory(pk, name, pk_cat, com, pay));

                dataGridView1.Rows.Add(name, pk);

            }

            

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            me = new Subcategory("Новая подкатегория",kat_father.pk_cat,0,0);
            string s = me.makeSQLinsert();
            cmd_pkat.CommandText = s;
            cmd_pkat.ExecuteNonQuery();

            s = "select max(pk_subcat) from subcategory";
            cmd_pkat.CommandText = s;
            dr_pkat = cmd_pkat.ExecuteReader();
            dr_pkat.Read();
            int pk_subcat = Convert.ToInt32(dr_pkat[0]);
            me.pk_subcat = pk_subcat;

            list_pcat.Add(me);
            dataGridView1.Rows.Add(me.name, me.pk_subcat);
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {

                    if (MessageBox.Show("Вы действительно хотите удалить выбранные подкатегории?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {

                        foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                        {
                            int cur_pk = Convert.ToInt32(row.Cells[1].Value.ToString());
                            me = list_pcat.Find(x => x.pk_subcat == cur_pk);
                            string s = me.makeSQLdelete();
                            cmd_pkat.CommandText = s;
                            cmd_pkat.ExecuteNonQuery();


                            list_pcat.Remove(me);

                        }


                        dataGridView1.Rows.Clear();

                        foreach (Subcategory c in list_pcat)
                        {
                            dataGridView1.Rows.Add(c.name, c.pk_subcat);


                        }
                    }
                }
                else            //если ничего не выделено, то удаляем текущую
                {
                    if (MessageBox.Show("Вы действительно хотите удалить данную категорию?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        int cur_pk = Convert.ToInt32(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                        me = list_pcat.Find(x => x.pk_subcat == cur_pk);
                        string s = me.makeSQLdelete();
                        cmd_pkat.CommandText = s;
                        cmd_pkat.ExecuteNonQuery();


                        list_pcat.Remove(me);



                        dataGridView1.Rows.Clear();

                        foreach (Subcategory c in list_pcat)
                        {
                            dataGridView1.Rows.Add(c.name, c.pk_subcat);


                        }

                    }



                } //MessageBox.Show("Сначала нужно выделить строки");
            }
            catch
            {
                MessageBox.Show("Сначала нужно все соответствующие товары и характеристики");
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            int cur_pk = Convert.ToInt32(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            me = list_pcat.Find(x => x.pk_subcat == cur_pk);
            Harakteristik p_kat = new Harakteristik(me);
            p_kat.ShowDialog();
            P_kategory_Load(sender, e);
        }
    }
}
