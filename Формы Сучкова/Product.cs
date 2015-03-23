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

        public Product(int pk_prod, int pk_act, int pk_subcat, string name, string sn, int pk_cheque, int min_inp_price, int comission, int pay_stay, int pk_stat, int expect_price, int finish_price, int flag_owner,int garant)
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

    }
}
