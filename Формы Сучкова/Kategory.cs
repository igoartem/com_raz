using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Формы_Сучкова
{
    public partial class Kategory : Form
    {
        public Kategory()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            P_kategory p_kat = new P_kategory();
            p_kat.Show();
        }
    }
}
