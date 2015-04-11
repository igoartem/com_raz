using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Формы_Сучкова
{
    public class Characteristic
    {
        public string name { get; set; }
        public int pk_char { get; set; }
        public int pk_subcat { get; set; }



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


        public string makeSQLinsert()
        {
            string s1 = "insert into Characteristic (pk_subcat, name) VALUES ('" + pk_subcat + "', '" + name +"' )";
            return s1;
        }

        public string makeSQLupdate()
        {
            string s1 = "update Characteristic SET name = '" + name + "', pk_subcat = '" + pk_subcat +"' where pk_char= " + pk_char;
            return s1;
        }

        public string makeSQLdelete()
        {
            string s1 = "delete Characteristic  where pk_char= " + pk_char;
            return s1;
        }
    }






}
