using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Формы_Сучкова
{
    class Subcategory
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
    }
}
