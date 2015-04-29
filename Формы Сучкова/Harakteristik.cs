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
    public partial class Harakteristik : Form
    {
        private Subcategory sub_father;
        private List<Characteristic> list_har;
        private Characteristic me;


        OracleCommand cmd_har;  // нужно для подключения к базе данных 
        OracleConnection con_har;
        OracleDataReader dr_har;

        public Harakteristik(Subcategory sub)
        {
            InitializeComponent();
            sub_father = sub;
            textBoxName.Text = sub_father.name;
            textBoxCom.Text = sub_father.comission.ToString();
            textBoxPay.Text = sub_father.pay_stay.ToString();
        }

        private void Harakteristik_Load(object sender, EventArgs e)
        {
            StreamReader sr;
            con_har = new OracleConnection(static_connect.con_str); // подключение к бд с логином admin и паролем 123
            cmd_har = new OracleCommand("", con_har);
            con_har.Open();

            cmd_har.CommandText = "SELECT * from CHARACTERISTIC WHERE PK_SUBCAT=" + sub_father.pk_subcat; // запрос на получение всех данных о категориях
            dr_har = cmd_har.ExecuteReader();

            list_har = new List<Characteristic>();


            while (dr_har.Read())
            {
                int pk = Convert.ToInt32(dr_har[0]);    //pk_char
                
                string name = dr_har[1].ToString();         //name
                int pk_subcat = Convert.ToInt32(dr_har[2]);
                list_har.Add(new Characteristic(name, pk_subcat, pk));

                dataGridView1.Rows.Add(name, pk);

            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            me = new Characteristic("Новая характеристика", sub_father.pk_subcat);
            string s = me.makeSQLinsert();
            cmd_har.CommandText = s;
            cmd_har.ExecuteNonQuery();

            s = "select max(pk_char) from Characteristic";
            cmd_har.CommandText = s;
            dr_har = cmd_har.ExecuteReader();
            dr_har.Read();
            int pk_char = Convert.ToInt32(dr_har[0]);
            me.pk_char = pk_char;

            list_har.Add(me);
            dataGridView1.Rows.Add(me.name, me.pk_char);
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {

                    if (MessageBox.Show("Вы действительно хотите удалить выбранные характеристики?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {

                        foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                        {
                            int cur_pk = Convert.ToInt32(row.Cells[1].Value.ToString());
                            me = list_har.Find(x => x.pk_char == cur_pk);
                            string s = me.makeSQLdelete();
                            cmd_har.CommandText = s;
                            cmd_har.ExecuteNonQuery();


                            list_har.Remove(me);

                        }


                        dataGridView1.Rows.Clear();

                        foreach (Characteristic c in list_har)
                        {
                            dataGridView1.Rows.Add(c.name, c.pk_char);


                        }
                    }
                }
                else            //если ничего не выделено, то удаляем текущую
                {
                    if (MessageBox.Show("Вы действительно хотите удалить данную характеристику?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        int cur_pk = Convert.ToInt32(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                        me = list_har.Find(x => x.pk_char == cur_pk);
                        string s = me.makeSQLdelete();
                        cmd_har.CommandText = s;
                        cmd_har.ExecuteNonQuery();


                        list_har.Remove(me);



                        dataGridView1.Rows.Clear();

                        foreach (Characteristic c in list_har)
                        {
                            dataGridView1.Rows.Add(c.name, c.pk_char);


                        }

                    }



                } //MessageBox.Show("Сначала нужно выделить строки");
            }
            catch
            {
                MessageBox.Show("Сначала нужно все соответствующие товары и характеристики");
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int cur_pk = Convert.ToInt32(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            string new_name = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            me = list_har.Find(x => x.pk_char == cur_pk);
            me.name = new_name;

            string ss = me.makeSQLupdate();

            cmd_har.CommandText = ss;
            cmd_har.ExecuteNonQuery();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

            sub_father.name = textBoxName.Text;
            sub_father.pay_stay = Convert.ToInt32(textBoxPay.Text);
            sub_father.comission = Convert.ToInt32(textBoxCom.Text);
            

            string ss = sub_father.makeSQLupdate();

            cmd_har.CommandText = ss;
            cmd_har.ExecuteNonQuery();
           // buttonSave.Enabled = false;
            groupBox2.Enabled = false;
            buttonEdit.Enabled = true;
        }

        private void buttonCans_Click(object sender, EventArgs e)
        {
            textBoxName.Text = sub_father.name;
            textBoxPay.Text = sub_father.pay_stay.ToString();
            textBoxCom.Text = sub_father.comission.ToString();
            //buttonSave.Enabled = false;
            groupBox2.Enabled = false;
            buttonEdit.Enabled = true;
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
           // buttonSave.Enabled = true;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            groupBox2.Enabled = true;
            buttonEdit.Enabled = false;
        }

        private void Harakteristik_FormClosed(object sender, FormClosedEventArgs e)
        {
            con_har.Close();
        }
    }
}
