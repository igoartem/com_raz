using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Формы_Сучкова
{
    class Inpit_act
    {
        public int pk_act { get; set; }
        public DateTime date_inp { get; set; }
        public DateTime date_end { get; set; }
        public int pk_sell { get; set; }
        public int pk_worker { get; set; }



        public Inpit_act(int pk_worker, int pk_sell)
        {
            this.pk_worker = pk_worker;
            this.pk_sell = pk_sell;
            date_inp = DateTime.Now;
        
        }


    }
}
