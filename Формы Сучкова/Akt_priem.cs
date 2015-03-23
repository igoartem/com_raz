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

namespace Формы_Сучкова
{
    public partial class Akt_priem : Form
    {
        List<Product> list_product;
        public Akt_priem()
        {
            InitializeComponent();
            list_product = new List<Product>();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fio, number_phone,passport, seller;
            int kol_days;
            DateTime dt;

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
                kol_days = Convert.ToInt32(textBox4.Text);
                dt = dateTimePicker1.Value;
                dt = dt.AddDays(kol_days);
                string date = Convert.ToString(dt.Day) + "." + Convert.ToString(dt.Month) + "." + Convert.ToString(dt.Year);
                dateTimePicker2.Value = DateTime.Parse(date);
            }


            
            
            
            //  INSERT INTO "ADMIN"."PRODUCT" (PK_ACT, PK_SUBCAT, NAME, SN, MIN_INP_PRICE, COMISSION, PAY_STAY, PK_STAT, EXPECT_PRICE) VALUES ('7', '3', '23343', '324234324', '3434', '2', '8', '112', '323434')

        }

        private void button3_Click(object sender, EventArgs e)
        {
            R_tovar form_priem = new R_tovar(this);
            form_priem.ShowDialog();
            Product prod = static_class.product;
            list_product.Add(prod);

            dataGridView1.Rows.Add();
            int kol_vo_row=dataGridView1.RowCount-1;

            if (list_product[list_product.Count - 1].flag_owner==1)
                dataGridView1.Rows[kol_vo_row].Cells[0].Value = true;
            dataGridView1.Rows[kol_vo_row].Cells[1].Value=list_product[list_product.Count-1].name.ToString();
            
            dataGridView1.Rows[kol_vo_row ].Cells[2].Value = list_product[list_product.Count - 1].pay_stay;
            dataGridView1.Rows[kol_vo_row ].Cells[3].Value = list_product[list_product.Count - 1].min_inp_price;
            dataGridView1.Rows[kol_vo_row ].Cells[4].Value = list_product[list_product.Count - 1].expect_price;
            dataGridView1.Rows[kol_vo_row ].Cells[5].Value = list_product[list_product.Count - 1].comission;
           // dataGridView1.Rows[kol_vo_row - 1].Cells[6].Value = list_product[list_product.Count - 1].name.ToString();
            
            
        }
    }
}
