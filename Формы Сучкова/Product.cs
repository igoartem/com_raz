using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Формы_Сучкова
{
    class Product
    {
        public int pk_prod { get; set; }
        public int pk_act { get; set; }
        public int pk_subcat { get; set; }
        public string name { get; set; }
        public string sn { get; set; }
        public int pk_cheque { get; set; }
        public int min_inp_price { get; set; }
        public int comission { get; set; }
        public int pay_stay { get; set; }
        public int pk_stat { get; set; }
        public int expect_price { get; set; }
        public int finish_price { get; set; }
        public int flag_owner { get; set; }
        public int garant { get; set; }

        public Product(int pk_prod, int pk_act, int pk_subcat, string name, string sn, int pk_cheque, int min_inp_price, int comission, int pay_stay, int pk_stat, int expect_price, int finish_price, int flag_owner, int garant)
        {
            this.pk_prod = pk_prod;
            this.pk_act = pk_act;
            this.pk_subcat = pk_subcat;
            this.name = name;
            this.sn = sn;
            this.pk_cheque = pk_cheque;
            this.min_inp_price = min_inp_price;
            this.comission = comission;
            this.pay_stay = pay_stay;
            this.pk_stat = pk_stat;
            this.expect_price = expect_price;
            this.finish_price = finish_price;
            this.flag_owner = flag_owner;
            this.garant = garant;
        }

        public Product(int pk_act, int pk_subcat, string name, string sn, int min_inp_price, int comission, int pay_stay, int expect_price, int flag_owner)
        {

            this.pk_act = pk_act;
            this.pk_subcat = pk_subcat;
            this.name = name;
            this.sn = sn;
            this.min_inp_price = min_inp_price;
            this.comission = comission;
            this.pay_stay = pay_stay;
            this.pk_stat = 21;                              //В продаже, при необходимости изменить
            this.expect_price = expect_price;
            this.flag_owner = flag_owner;
            //this.garant = garant;
        }

        public Product(int pk_subcat, string name, string sn, int min_inp_price, int comission, int pay_stay, int expect_price, int flag_owner)
        {
            this.pk_subcat = pk_subcat;
            this.name = name;
            this.sn = sn;
            this.min_inp_price = min_inp_price;
            this.comission = comission;
            this.pay_stay = pay_stay;
            this.pk_stat = 21;                              //В продаже, при необходимости изменить
            this.expect_price = expect_price;
            this.flag_owner = flag_owner;
        }

        public string makeSQLinsert()
        {
            string s1 = "insert into Product ( ";

            string s2 = ") VALUES (";
            if (pk_act != 0)
            {
                s1 += "pk_act,";
                s2 += pk_act + " ,";
            }
            if (pk_subcat != 0)
            {
                s1 += " pk_subcat,";
                s2 += pk_subcat + " ,";
            }
            if (name != "")
            {
                s1 += " name,";
                s2 += "'" + name + "' ,";
            }
            if (sn != "")
            {
                s1 += " sn,";
                s2 += "'" + sn + "' ,";
            }
            if (pk_cheque != 0)
            {
                s1 += " pk_cheque,";
                s2 += pk_cheque + " ,";
            }
            if (min_inp_price != 0)
            {
                s1 += " min_inp_price,";
                s2 += min_inp_price + " ,";
            }
            if (comission != 0)
            {
                s1 += " comission,";
                s2 += comission + " ,";
            }
            if (pay_stay != 0)
            {
                s1 += " pay_stay,";
                s2 += pay_stay + " ,";
            }
            if (pk_stat != 0)
            {
                s1 += " pk_stat,";
                s2 += pk_stat + " ,";
            }
            if (expect_price != 0)
            {
                s1 += " expect_price,";
                s2 += expect_price + " ,";
            }
            if (finish_price != 0)
            {
                s1 += " finish_price," + " ,";
                s2 += finish_price + " ,";
            }
            
                s1 += " flag_owner,";
                s2 += flag_owner + " ,";
            

            if (garant != 0)
            {
                s1 += " garant,";
                s2 += garant + " ,";
            }
            s2 = s2.Substring(0, s2.Length - 1);       //затёр ,
            s1 = s1.Substring(0, s1.Length - 1);
            s2 += ")";

            string s3 = s1 + s2;
            return s3;
        }

        public string makeSQLupdate()
        {
            string s1 = "update Product SET ";
            if (pk_subcat != 0)
            {
                s1 += " pk_subcat = " + pk_subcat + ",";   
            }
            if (name != "")
            {
                s1 += " name = '" + name + "',";
            }
            if (sn != "")
            {
                s1 += " sn = '" + sn + "',";
            }
            if (min_inp_price != 0)
            {
                s1 += " min_inp_price = " + min_inp_price + ",";
            }
            if (comission != 0)
            {
                s1 += " comission = " + comission + ",";
            }
            if (pay_stay != 0)
            {
                s1 += " pay_stay = " + pay_stay + ",";
            }
            if (pk_cheque != 0)
            {
                s1 += " pk_cheque = " + pk_cheque + ",";
            }
            if (pk_stat != 0)
            {
                s1 += " pk_stat = " + pk_stat + ",";
            }
            if (expect_price != 0)
            {
                s1 += " expect_price = " + expect_price + ",";
            }
            if (finish_price != 0)
            {
                s1 += " finish_price = " + finish_price + ",";
            }
            if (flag_owner != 0)
            {
                s1 += " flag_owner = " + flag_owner + ",";
            }
            if (garant != 0)
            {
                s1 += " garant = " + garant + ",";
            }

            s1 = s1.Substring(0, s1.Length - 1);

            s1 += " where pk_prod = " + pk_prod;
            return s1;
        }

    }
}
