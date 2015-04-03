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
    }
}
