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
            cmd_zay.CommandText = "SELECT * from REQUEST"; // запрос на получение всех данных о категориях
            dr_zay = cmd_zay.ExecuteReader();

            list_zay = new List<Request>();



            while (dr_zay.Read())
            {
                int pk = Convert.ToInt32(dr_zay[0]);
                DateTime dateQ = DateTime.Now;
                int pk_appl = Convert.ToInt32(dr_zay[2]);
                int pk_subcat = Convert.ToInt32(dr_zay[3]);
                string about = dr_zay[4].ToString();
                list_zay.Add(new Request(pk, pk_appl,pk_subcat,about,dateQ));

               // dataGridView1.Rows.Add(name, pk);

            }
          
        }
    }
}
