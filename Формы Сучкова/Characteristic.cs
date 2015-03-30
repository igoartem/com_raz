using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Формы_Сучкова
{
    class Characteristic
    {
        string name;
        int pk_char;
        int pk_subcat;



        public Characteristic(string name, int pk_subcat)
        {
            this.name = name;
            this.pk_subcat = pk_subcat;

        }


        public Characteristic(string name, int pk_subcat, int pk_char)
        {
            this.name = name;
            this.pk_subcat = pk_subcat;
            this.pk_char = pk_char;
        }

    }






}
