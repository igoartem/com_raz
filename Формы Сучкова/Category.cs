using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Формы_Сучкова
{
    class Category
    {
        public int pk_cat { get; set; }
        public string name { get; set; }

        public Category(string name)
        {
            this.name = name;
        
        
        }
    }
}
