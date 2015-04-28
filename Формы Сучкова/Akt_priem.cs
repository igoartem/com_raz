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
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;

using Application = Microsoft.Office.Interop.Excel.Application;



namespace Формы_Сучкова
{
    public partial class Akt_priem : Form
    {
        List<Product> list_product;
        List<elemOfConfTable> list_elem;
        OracleCommand cmd_akt_priem;     //
        OracleConnection con_akt_priem;  // Подключение для вывода товаров в таблицу
        OracleDataReader dr_akt_priem;   //
        int pk = -1;
        private Application application;
        private Workbook workBook;
        private Worksheet worksheet;


        public void connect()
        {
            list_product = new List<Product>();
            list_elem = new List<elemOfConfTable>();
            StreamReader sr;        // загрузка файла с адресом хоста с бд
            string s = null;
            try
            {
                if ((sr = new StreamReader(@"ip_base.txt")) != null)
                    s = sr.ReadLine();
            }

            catch (Exception)
            {
                MessageBox.Show("Error Base!");
            }
            con_akt_priem = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + s + ")(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = XE))); User Id=" + "admin" + ";Password=" + "123" + ";");
            cmd_akt_priem = new OracleCommand("", con_akt_priem);
            con_akt_priem.Open();
        }

        public Akt_priem()
        {
            InitializeComponent();
            connect();

        }

        public Akt_priem(R_tovar my,int pk_act)
        {
            pk = pk_act;
            InitializeComponent();
            connect();
            button1.Visible = false;
            button3.Visible = false;
            button4.Visible = true;
        }

        public Akt_priem(R_tovar_arh my, int pk_act)
        {
            pk = pk_act;
            InitializeComponent();
            connect();
            button1.Visible = false;
            button3.Visible = false;
            button2.Visible = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox6.Enabled = false;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            button4.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fio, number_phone, passport, seller;
           // int kol_days;
            DateTime date_fin, date_start;

            if (textBox1.Text == "")
            {
                MessageBox.Show("Не заполнено поле ФИО!");
                return;
            }
            else
                fio = textBox1.Text;
            if (textBox2.Text == "")
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
                date_start = dateTimePicker1.Value;
                date_fin = dateTimePicker2.Value;
            }
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Не добавлено ни одного товара!");
                return;
            }
            Input_act inp_act;

            if (pk < 0)
            {
                string ss = "INSERT INTO SELLER (FIO, PASSPORT, PHONE) VALUES ('" + fio + "', '" + passport + "','" + number_phone + "')";
                cmd_akt_priem.CommandText = ss;
                cmd_akt_priem.ExecuteNonQuery();

                ss = "select pk_sell from seller where fio='" + fio + "' and passport ='" + passport + "' and phone='" + number_phone + "'";
                cmd_akt_priem.CommandText = ss;
                dr_akt_priem = cmd_akt_priem.ExecuteReader();
                dr_akt_priem.Read();
                int pk_sell = Convert.ToInt32(dr_akt_priem[0]);

                inp_act = new Input_act(static_class.worker, pk_sell); //здесь надо будет менять работника

                inp_act.date_inp = date_start;
                inp_act.date_end = date_fin;
                ss = "INSERT INTO INPUT_ACT (DATE_INP, DATE_END, PK_SELL, PK_WORKER) VALUES (TO_DATE('" + inp_act.date_inp.ToString() + "','DD.MM.YYYY HH24:MI:SS'), TO_DATE('" + inp_act.date_end.ToString() + "','DD.MM.YYYY HH24:MI:SS'),'" + inp_act.pk_sell + "', '" + inp_act.pk_worker + "')";

                cmd_akt_priem.CommandText = ss;
                cmd_akt_priem.ExecuteNonQuery();

                ss = "SELECT PK_ACT From INPUT_ACT where PK_SELL='" + inp_act.pk_sell + "' AND pk_worker='" + inp_act.pk_worker + "' and date_INP=TO_DATE('" + inp_act.date_inp.ToString() + "','DD.MM.YYYY HH24:MI:SS') and date_end=TO_DATE('" + inp_act.date_end.ToString() + "','DD.MM.YYYY HH24:MI:SS')";
                cmd_akt_priem.CommandText = ss;
                dr_akt_priem = cmd_akt_priem.ExecuteReader();
                dr_akt_priem.Read();

                int pk_inp_act = Convert.ToInt32(dr_akt_priem[0]);


                // dr1 = cmd1.ExecuteReader();
                int kol_vo_tov = list_product.Count;
                for (int i = 0; i < kol_vo_tov; i++)
                {
                    list_product[i].pk_act = pk_inp_act;
                    ss = list_product[i].makeSQLinsert();
                    cmd_akt_priem.CommandText = ss;
                    cmd_akt_priem.ExecuteNonQuery();

                    ss = "select PK_PROD from product where PK_ACT='" + list_product[i].pk_act +
                        "' AND PK_SUBCAT='" + list_product[i].pk_subcat +
                        "' and NAME='" + list_product[i].name + 
                        "' and SN='" + list_product[i].sn +
                        "' and MIN_INP_PRICE='"+ list_product[i].min_inp_price +
                        "' and COMISSION= '"+ list_product[i].comission+
                        "' and PAY_STAY= '"+ list_product[i].pay_stay+
                        "' and PK_STAT= '"+ list_product[i].pk_stat+
                        "' and EXPECT_PRICE= '"+ list_product[i].expect_price+
                        "' and FLAG_OWNER= '"+ list_product[i].flag_owner+
                        "' and OPISANIE= '"+ list_product[i].opisanie+
                        "'";
                    cmd_akt_priem.CommandText = ss;
                    dr_akt_priem = cmd_akt_priem.ExecuteReader();
                    dr_akt_priem.Read();
                    int pk_tov = Convert.ToInt32(dr_akt_priem[0]);
                    for (int j = 0; j < list_elem.Count; j++)
                    {
                        if (list_elem[j].pk_prod == i)
                        {
                            list_elem[j].pk_prod = pk_tov;
                            ss=list_elem[j].makeSQLinsert();
                            cmd_akt_priem.CommandText = ss;
                            cmd_akt_priem.ExecuteNonQuery();
                        }
                    }
                    //получаем ПК добавленного товара товара 
                }
            }
            else
            {
                cmd_akt_priem.CommandText = "UPDATE seller set FIO = '" + fio + "', phone = '" + number_phone + "', PASSPORT = '" + passport + "' where seller.pk_sell = (select input_act.pk_sell from input_act where input_act.pk_act = '" + pk.ToString() + "')";
                cmd_akt_priem.ExecuteNonQuery();

                cmd_akt_priem.CommandText = "UPDATE input_act set date_end = TO_DATE('" + date_fin.ToString() + "','DD.MM.YYYY HH24:MI:SS') where input_act.pk_act = '" + pk.ToString() + "'";
                cmd_akt_priem.ExecuteNonQuery();
                dr_akt_priem.Read();
            }

           
            DialogResult result;

           result= MessageBox.Show("Распечатать акт приемки?", "Акт приемки", MessageBoxButtons.YesNo);
           if (result == System.Windows.Forms.DialogResult.Yes)
           {
               string zap = "select max(PK_ACT) from input_act";
               cmd_akt_priem.CommandText = zap;
               dr_akt_priem = cmd_akt_priem.ExecuteReader();
               dr_akt_priem.Read();
               int pk_ = Convert.ToInt32(dr_akt_priem[0]);

               zap = "select fio from worker where  PK_WORKER ="+static_class.worker;
               cmd_akt_priem.CommandText = zap;
               dr_akt_priem = cmd_akt_priem.ExecuteReader();
               dr_akt_priem.Read();
               string fio_work=dr_akt_priem[0].ToString();
               application = new Application { Visible = true, DisplayAlerts = false };


               string template = "primer.xlsx";
               workBook = application.Workbooks.Open(Path.Combine(Environment.CurrentDirectory, template));
               worksheet = workBook.ActiveSheet as Worksheet;
               worksheet.Range["C1"].Value = pk_;
               worksheet.Range["C2"].Value = fio;
               worksheet.Range["C3"].Value = passport;
               worksheet.Range["C4"].Value = number_phone;
               worksheet.Range["C5"].Value = fio_work;
               worksheet.Range["C6"].Value =  date_start.ToString();
               worksheet.Range["C7"].Value = date_fin.ToString();

               zap = "select NAME,sn,MIN_INP_PRICE,COMISSION,EXPECT_PRICE,FLAG_OWNER,PAY_STAY from product where PK_ACT=" + pk_;
               cmd_akt_priem.CommandText = zap;
               dr_akt_priem = cmd_akt_priem.ExecuteReader();
               

               int nn = 1;
               int ne=10;
               while (dr_akt_priem.Read())
               {
                   worksheet.Range["B" + ne].Value = nn;
                   if(dr_akt_priem[5].ToString()=="1")
                       worksheet.Range["C"+ne].Value = "+";
                   else
                       worksheet.Range["C"+ne].Value = "-";

                   worksheet.Range["D" + ne].Value = dr_akt_priem[0].ToString();
                   worksheet.Range["E" + ne].Value = dr_akt_priem[6].ToString();
                   worksheet.Range["F" + ne].Value = dr_akt_priem[2].ToString();
                   worksheet.Range["G" + ne].Value = dr_akt_priem[4].ToString();
                   worksheet.Range["H" + ne].Value = dr_akt_priem[3].ToString();
                   worksheet.Range["I" + ne].Value = dr_akt_priem[1].ToString();

                   ne++;
                   nn++;
               }


               string savedFileName = "book"+pk_+".xlsx";
               workBook.SaveAs(Path.Combine(Environment.CurrentDirectory, savedFileName));
              // CloseExcel();

           }
            this.Close();

        }

        private void CloseExcel()
        {
            if (application != null)
            {


                int excelProcessId = -1;
                GetWindowThreadProcessId(application.Hwnd, ref excelProcessId);

                Marshal.ReleaseComObject(worksheet);
                workBook.Close();
                Marshal.ReleaseComObject(workBook);
                application.Quit();
                Marshal.ReleaseComObject(application);

                application = null;
                // Прибиваем висящий процесс
                try
                {
                    Process process = Process.GetProcessById(excelProcessId);
                    process.Kill();
                }
                finally { }
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(int hWnd, ref int lpdwProcessId);

        private void button3_Click(object sender, EventArgs e)
        {
            R_tovar form_priem = new R_tovar(this);
            form_priem.ShowDialog();

            
            Product prod = static_class.product;

            List<elemOfConfTable> list_el = static_class.list_char;
            if (prod != null)
            {
                list_product.Add(prod);
                add_datagrid(list_product[list_product.Count - 1]);
                for (int i = 0; i < list_el.Count; i++)
                {
                    list_el[i].pk_prod = list_product.Count - 1;
                    list_elem.Add(list_el[i]);
                }
                    static_class.product = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
            {
                MessageBox.Show("Не выбран товар для удаления!");
                return;

            }
            else
            {
                list_product.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                
                list_elem.RemoveAll(elemOfConfTable => elemOfConfTable.pk_prod == dataGridView1.CurrentCell.RowIndex);


                update_list_elem(dataGridView1.CurrentCell.RowIndex);
                    update_datagrid();
            }
        }

        private void add_datagrid(Product prod)
        {
            dataGridView1.Rows.Add();
            int kol_vo_row = dataGridView1.RowCount - 1;

            if (list_product[list_product.Count - 1].flag_owner == 1)
                dataGridView1.Rows[kol_vo_row].Cells[0].Value = true;
            dataGridView1.Rows[kol_vo_row].Cells[1].Value = prod.name.ToString();
            dataGridView1.Rows[kol_vo_row].Cells[2].Value = prod.pay_stay;
            dataGridView1.Rows[kol_vo_row].Cells[3].Value = prod.min_inp_price;
            dataGridView1.Rows[kol_vo_row].Cells[4].Value = prod.expect_price;
            dataGridView1.Rows[kol_vo_row].Cells[5].Value = prod.comission;


        }

        private void update_datagrid()
        {

            dataGridView1.Rows.Clear();
            int kol_vo = list_product.Count;
            for (int i = 0; i < kol_vo; i++)
            {
                add_datagrid(list_product[i]);
            }
        }

        private void update_list_elem(int pk_del)
        {

            for (int i = 0; i < list_elem.Count; i++)
                if (list_elem[i].pk_prod > pk_del)
                    list_elem[i].pk_prod--;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //dateTimePicker2.Enabled = false;

            int kol_days = 0;
            //if (textBox4.Text != "" && textBox4.Text.Length < 6)
            try 
            { 
                kol_days = Convert.ToInt32(textBox4.Text);
                if (kol_days < 0 || kol_days > 1000)
                    kol_days = 0;
            
            }
            catch { }
            DateTime dt = dateTimePicker1.Value;
            dt = dt.AddDays(kol_days);
            dateTimePicker2.Value = dt;
            
            //dateTimePicker2.Enabled = true;
        }

        private void Akt_priem_Load(object sender, EventArgs e)
        {
            textBox6.Text = static_class.worker_fio;
            if (pk >= 0)
            {

                cmd_akt_priem.CommandText = "select seller.FIO, seller.PHONE, seller.PASSPORT, input_act.DATE_INP, input_act.DATE_END from input_act, seller where input_act.PK_ACT = " + pk.ToString() + " and input_act.PK_SELL = seller.PK_SELL";
                dr_akt_priem = cmd_akt_priem.ExecuteReader();
                dr_akt_priem.Read();

                textBox1.Text = dr_akt_priem[0].ToString();
                textBox2.Text = dr_akt_priem[1].ToString();
                textBox3.Text = dr_akt_priem[2].ToString();

                dateTimePicker1.Value = Convert.ToDateTime(dr_akt_priem[3].ToString());
                dateTimePicker2.Value = Convert.ToDateTime(dr_akt_priem[4].ToString());

               

                cmd_akt_priem.CommandText = "select product.FLAG_OWNER, product.NAME, product.PAY_STAY, product.MIN_INP_PRICE, product.EXPECT_PRICE, product.COMISSION from input_act, product where input_act.PK_ACT = " + pk + " and product.PK_ACT = input_act.PK_ACT";
                dr_akt_priem = cmd_akt_priem.ExecuteReader();

                dataGridView1.Enabled = false;
             
                while (dr_akt_priem.Read())
                {
                    dataGridView1.Rows.Add();

                    if (Convert.ToInt32(dr_akt_priem[0]) == 1)
                    {
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = true;
                       
                    }
                    

                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = dr_akt_priem[1].ToString(); // naimenov
                    

                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = dr_akt_priem[2].ToString(); //FIO
                    
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = dr_akt_priem[3].ToString(); //garant
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = dr_akt_priem[4].ToString(); //exp_cost
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = dr_akt_priem[5].ToString(); // Fin_price
                    
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].ReadOnly = true;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].ReadOnly = true;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].ReadOnly = true;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].ReadOnly = true;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].ReadOnly = true;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].ReadOnly = true;
                }

                cmd_akt_priem.CommandText = "select product_ar.FLAG_OWNER, product_ar.NAME, product_ar.PAY_STAY, product_ar.MIN_INP_PRICE, product_ar.EXPECT_PRICE, product_ar.COMISSION from input_act, product_ar where input_act.PK_ACT = " + pk + " and product_ar.PK_ACT = input_act.PK_ACT";
                dr_akt_priem = cmd_akt_priem.ExecuteReader();

                dataGridView1.Enabled = false;
                while (dr_akt_priem.Read())
                {
                    dataGridView1.Rows.Add();

                    if (Convert.ToInt32(dr_akt_priem[0]) == 1)
                    {
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = true;
                    }

                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = dr_akt_priem[1].ToString(); // naimenov
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = dr_akt_priem[2].ToString(); //FIO
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = dr_akt_priem[3].ToString(); //garant
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = dr_akt_priem[4].ToString(); //exp_cost
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = dr_akt_priem[5].ToString(); // Fin_price
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].ReadOnly = true;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].ReadOnly = true;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].ReadOnly = true;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].ReadOnly = true;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].ReadOnly = true;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].ReadOnly = true;
                }

                dataGridView1.Enabled = true;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            string ss = Convert.ToString((dateTimePicker2.Value - dateTimePicker1.Value).Days);
            //textBox4.Enabled = false;
            textBox4.Text = ss;
            //textBox4.Enabled = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string zap;
            int pk_ = pk;
            zap = "select worker.FIO from input_act,worker where worker.PK_WORKER=input_act.PK_WORKER and pk_act=" + pk;
            cmd_akt_priem.CommandText = zap;
            dr_akt_priem = cmd_akt_priem.ExecuteReader();
            dr_akt_priem.Read();
            string fio_work = dr_akt_priem[0].ToString();

            //zap = "select fio from worker where  PK_WORKER =" + ;
            //cmd_akt_priem.CommandText = zap;
            //dr_akt_priem = cmd_akt_priem.ExecuteReader();
            //dr_akt_priem.Read();
            //string fio_work = dr_akt_priem[0].ToString();
            application = new Application { Visible = true, DisplayAlerts = false };

            zap = "select seller.FIO,seller.PASSPORT,seller.PHONE,input_act.DATE_INP,input_act.DATE_END from input_act,seller,worker where seller.PK_SELL = input_act.PK_SELL and worker.PK_WORKER = input_act.PK_WORKER and input_act.PK_ACT="+pk_;
            cmd_akt_priem.CommandText = zap;
            dr_akt_priem = cmd_akt_priem.ExecuteReader();
            dr_akt_priem.Read();

            string template = "primer.xlsx";
            workBook = application.Workbooks.Open(Path.Combine(Environment.CurrentDirectory, template));
            worksheet = workBook.ActiveSheet as Worksheet;
            worksheet.Range["C1"].Value = pk_;
            worksheet.Range["C2"].Value = dr_akt_priem[0].ToString();
            worksheet.Range["C3"].Value = dr_akt_priem[1].ToString();
            worksheet.Range["C4"].Value = dr_akt_priem[2].ToString();
            worksheet.Range["C5"].Value = fio_work;
            worksheet.Range["C6"].Value = dr_akt_priem[3].ToString();
            worksheet.Range["C7"].Value = dr_akt_priem[4].ToString();

            zap = "select NAME,sn,MIN_INP_PRICE,COMISSION,EXPECT_PRICE,FLAG_OWNER,PAY_STAY from product where PK_ACT=" + pk_;
            cmd_akt_priem.CommandText = zap;
            dr_akt_priem = cmd_akt_priem.ExecuteReader();


            int nn = 1;
            int ne = 10;
            while (dr_akt_priem.Read())
            {
                worksheet.Range["B" + ne].Value = nn;
                if (dr_akt_priem[5].ToString() == "1")
                    worksheet.Range["C" + ne].Value = "+";
                else
                    worksheet.Range["C" + ne].Value = "-";

                worksheet.Range["D" + ne].Value = dr_akt_priem[0].ToString();
                worksheet.Range["E" + ne].Value = dr_akt_priem[6].ToString();
                worksheet.Range["F" + ne].Value = dr_akt_priem[2].ToString();
                worksheet.Range["G" + ne].Value = dr_akt_priem[4].ToString();
                worksheet.Range["H" + ne].Value = dr_akt_priem[3].ToString();
                worksheet.Range["I" + ne].Value = dr_akt_priem[1].ToString();

                ne++;
                nn++;
            }


            string savedFileName = "book" + pk_ + ".xlsx";
            workBook.SaveAs(Path.Combine(Environment.CurrentDirectory, savedFileName));
            //CloseExcel();
        }
    }
}
