using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.IO;

namespace Формы_Сучкова
{
    public partial class Statistic : Form
    {
        OracleCommand cmd_zay;  // нужно для подключения к базе данных 
        OracleConnection con_zay;
        OracleDataReader dr_zay;


        public Statistic()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //-------------------------------прибыль------------------------
            int sum1 = 0,sum2=0;
            string Date1 = dateTimePicker1.Value.ToString().Substring(0,10);
            string Date2 = dateTimePicker2.Value.AddDays(1).ToString().Substring(0, 10);        //добавлен день чтобы считалось до начала следующего дня
            //cmd_zay.CommandText = "select T.finish_price - T.min_inp_price from PRODUCT T, CHEQUE CH where T.pk_stat = 22 AND T.pk_cheque = CH.pk_cheque AND T.flag_owner > 0 AND CH.Date_ch > TO_DATE('" + Date1 + "','DD.MM.YYYY HH24:MI:SS') AND CH.Date_ch < TO_DATE('" + Date2 + "','DD.MM.YYYY HH24:MI:SS')";
            cmd_zay.CommandText = "select T.finish_price - T.min_inp_price from PRODUCT T, CHEQUE CH where T.pk_stat = 22 AND T.pk_cheque = CH.pk_cheque AND T.flag_owner > 0 AND CH.Date_ch > TO_DATE('" + Date1 + "','DD.MM.YYYY') AND CH.Date_ch < TO_DATE('" + Date2 + "','DD.MM.YYYY')";
            dr_zay = cmd_zay.ExecuteReader();
            while (dr_zay.Read())
            {

                sum1 += Convert.ToInt32(dr_zay[0]);
            }

            //cmd_zay.CommandText = "select  T.finish_price - T.min_inp_price from PRODUCT_AR  T, CHEQUE CH where T.pk_cheque = CH.pk_cheque AND T.flag_owner > 0 AND CH.Date_ch > TO_DATE('" + Date1 + "','DD.MM.YYYY HH24:MI:SS') AND CH.Date_ch < TO_DATE('" + Date2 + "','DD.MM.YYYY HH24:MI:SS')";
            cmd_zay.CommandText = "select  T.finish_price - T.min_inp_price from PRODUCT_AR  T, CHEQUE CH where T.pk_cheque = CH.pk_cheque AND T.flag_owner > 0 AND CH.Date_ch > TO_DATE('" + Date1 + "','DD.MM.YYYY') AND CH.Date_ch < TO_DATE('" + Date2 + "','DD.MM.YYYY')";
            dr_zay = cmd_zay.ExecuteReader();
            while (dr_zay.Read())
            {

                sum2 += Convert.ToInt32(dr_zay[0]);
            }
            int sumPrib = sum1 + sum2;


            int sumCom = 0; 
            int sumPay = 0;
            //---------------------------------------commission------------------------

            cmd_zay.CommandText = "select T.finish_price * T.comission / 100, T.pay_stay, CH.Date_ch, I.date_inp from PRODUCT T, CHEQUE CH, INPUT_ACT I where T.pk_stat = 22 AND T.pk_act = I.pk_act AND T.pk_cheque = CH.pk_cheque AND T.flag_owner = 0 AND CH.Date_ch > TO_DATE('" + Date1 + "','DD.MM.YYYY') AND CH.Date_ch < TO_DATE('" + Date2 + "','DD.MM.YYYY')";
            dr_zay = cmd_zay.ExecuteReader();
            while (dr_zay.Read())
            {

                sumCom += Convert.ToInt32(dr_zay[0]);
                int payForDay = Convert.ToInt32(dr_zay[1]);
                DateTime d1 = Convert.ToDateTime(dr_zay[2]);
                DateTime d2 = Convert.ToDateTime(dr_zay[3]);
                //DateTime.
                TimeSpan d =  d1 - d2;
                sumPay += d.Days * payForDay;
               // d1.CompareTo(d2);
            }


            cmd_zay.CommandText = "select T.finish_price * T.comission / 100, T.pay_stay, CH.Date_ch, I.date_inp from PRODUCT_AR T, CHEQUE CH, INPUT_ACT I where T.pk_stat = 22 AND T.pk_act = I.pk_act AND T.pk_cheque = CH.pk_cheque AND T.flag_owner = 0 AND CH.Date_ch > TO_DATE('" + Date1 + "','DD.MM.YYYY') AND CH.Date_ch < TO_DATE('" + Date2 + "','DD.MM.YYYY')";
            dr_zay = cmd_zay.ExecuteReader();
            while (dr_zay.Read())
            {

                sumCom += Convert.ToInt32(dr_zay[0]);
                int payForDay = Convert.ToInt32(dr_zay[1]);
                DateTime d1 = Convert.ToDateTime(dr_zay[2]);
                DateTime d2 = Convert.ToDateTime(dr_zay[3]);
                //DateTime.
                TimeSpan d = d1 - d2;
                sumPay += d.Days * payForDay;
                // d1.CompareTo(d2);
            }





            //---------------------------------------output------------------------
            textBox1.Text = (sumCom).ToString();
            textBox2.Text = (sumPay).ToString();
            textBox3.Text = (sumPrib).ToString();
            textBox4.Text = (sumPrib + sumCom + sumPay).ToString();
        }

        private void Statistic_Load(object sender, EventArgs e)
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
            //static_connect.con_str = con_str;
            con_zay = new OracleConnection(con_str); // подключение к бд с логином admin и паролем 123
            cmd_zay = new OracleCommand("", con_zay);
            con_zay.Open();  

        }

        private void Statistic_FormClosed(object sender, FormClosedEventArgs e)
        {
            con_zay.Close();
        }
    }
}
