using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Формы_Сучкова
{
    public class Category
    {
        public int pk_cat { get; set; }
        public string name { get; set; }

        public Category(string name)
        {
            this.name = name;
        
        
        }
        public Category(string name,int pk_cat)
        {
            this.name = name;
            this.pk_cat = pk_cat; 

        }

        public string makeSQLinsert()
        {
            string s1 = "insert into Category (name) VALUES ('" + name + "')";
            return s1;
        }

        public string makeSQLupdate()
        {
            string s1 = "update Category SET name = '" + name + "' where pk_cat= " + pk_cat;
            return s1;
        }

        public string makeSQLdelete()
        {
            string s1 = "delete Category  where pk_cat= " + pk_cat;
            return s1;
        }

    }
}
