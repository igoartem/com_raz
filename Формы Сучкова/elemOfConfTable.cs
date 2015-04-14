using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Формы_Сучкова
{
    class elemOfConfTable
    {
        public string value { get; set; }
        public int pk_char { get; set; }
        public int pk_prod { get; set; }
        public int pk_tab { get; set; }
        public string name_char { get; set; }

        public elemOfConfTable(string value, int pk_char)
        {
            this.value = value;
            this.pk_char = pk_char;
         

        }

        public elemOfConfTable(string value, int pk_char,int pk_prod)
        {
            this.value = value;
            this.pk_char = pk_char;
            this.pk_prod = pk_prod;

        }

        public elemOfConfTable(string value, int pk_char, int pk_prod, int pk_tab,string name_char)
        {
            this.value = value;
            this.pk_char = pk_char;
            this.pk_prod = pk_prod;
            this.pk_tab = pk_tab;
            this.name_char = name_char;
        }

        public elemOfConfTable(int pk_tab, string value, int pk_prod, int pk_char)
        {
            this.value = value;
            this.pk_char = pk_char;
            this.pk_prod = pk_prod;
            this.pk_tab = pk_tab;
        }

        public string makeSQLinsert_AR()
        {
            string s1 = "insert into TABLE_CONFORM_AR (PK_TAB_AR, VALUE, PK_PROD_AR, PK_CHAR) VALUES ('" + pk_tab + "','" + value + "', '" + pk_prod + "','" + pk_char + "' )";
            return s1;
        }

        public string makeSQLinsert()
        {
            string s1 = "insert into TABLE_CONFORM (VALUE, PK_PROD, PK_CHAR) VALUES ('" + value + "', '" + pk_prod + "','"+pk_char+"' )";
            return s1;
        }

        public string makeSQLupdate()
        {
            string s1 = "update TABLE_CONFORM SET VALUE = '" + value + "', PK_PROD = '" + pk_prod + "', PK_CHAR='"+pk_char+"' where pk_tab= " + pk_tab;
            return s1;
        }

        public string makeSQLdelete()
        {
            string s1 = "delete TABLE_CONFORM  where pk_tab= " + pk_tab;
            return s1;
        }

    }
}
