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
        //private Subcategory me;


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
                int pk = Convert.ToInt32(dr_har[0]);
                
                string name = dr_har[1].ToString();
                int pk_subcat = Convert.ToInt32(dr_har[2]);
                list_har.Add(new Characteristic(name, pk_subcat, pk));

                dataGridView1.Rows.Add(name, pk);

            }
        }
    }
}
