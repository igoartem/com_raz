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
    public partial class Prodaja : Form
    {
        public int[] mass_pk;
        public Prodaja(int[] pk)
        {
            mass_pk = pk;
            InitializeComponent();
        }

        private void Prodaja_Load(object sender, EventArgs e)
        {

        }
    }
}
