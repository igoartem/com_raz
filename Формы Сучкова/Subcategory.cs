using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Формы_Сучкова
{
    public class Subcategory
    {
        public int pk_subcat { get; set; }
        public int pk_cat { get; set; }
        public string name { get; set; }
        public int comission { get; set; }
        public int pay_stay { get; set; }


        public Subcategory(string name, int pk_cat, int comission,int pay_stay)
        {
            this.name = name;
            this.pk_cat = pk_cat;
            this.comission = comission;
            this.pay_stay = pay_stay;
        
        
        }
        public Subcategory(int pk_subcat,string name, int pk_cat, int comission, int pay_stay)
        {
            this.pk_subcat = pk_subcat;
            this.name = name;
            this.pk_cat = pk_cat;
            this.comission = comission;
            this.pay_stay = pay_stay;


        }




        public string makeSQLinsert()
        {
            string s1 = "insert into Subcategory (pk_cat, name, comission, pay_stay) VALUES ('" + pk_cat + "', '" + name + "', '" + comission + "', '" + pay_stay + "' )";
            return s1;
        }

        public string makeSQLupdate()
        {
            string s1 = "update Subcategory SET name = '" + name + "', pk_cat = '" +  pk_cat + "', comission = '" + comission + "', pay_stay = '" + pay_stay + "' where pk_subcat= " + pk_subcat;
            return s1;
        }

        public string makeSQLdelete()
        {
            string s1 = "delete Subcategory  where pk_subcat= " + pk_subcat;
            return s1;
        }
    }
}
