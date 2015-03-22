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

namespace Формы_Сучкова
{
    public partial class Tovari : Form
    {
        public Tovari()
        {
            InitializeComponent();
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
            StreamReader sr;
            string s;
            try
            {
                if ((sr = new StreamReader(@"ip_base.txt")) != null)
                    s = sr.ReadLine();
            }
            
            catch (Exception ex)
            {
                MessageBox.Show("Error Base!");
            }
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

        private void button6_Click(object sender, EventArgs e)
        {
            Akt_priem add_tovar = new Akt_priem();
            add_tovar.Show();
        }
    }
}
