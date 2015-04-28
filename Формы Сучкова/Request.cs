using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Формы_Сучкова
{
    public class Request
    {
        public int pk_request { get; set; }
        public string FIO { get; set; }
        public string phone { get; set; }
        public int pk_subcat { get; set; }
        public int price { get; set; }
        public string about { get; set; }
        public DateTime Date_req { get; set; }


        public Request(int pk_request, string FIO, int pk_subcat, string about, DateTime Date_req, string phone,int price)
        {
            this.pk_request = pk_request;
            this.FIO = FIO;
            this.phone = phone;
            this.pk_subcat = pk_subcat;
            this.about = about;
            this.Date_req = Date_req;
            this.price = price;
        }

        public Request( string FIO, int pk_subcat, string about, DateTime Date_req, string phone, int price)
        {
           // this.pk_request = pk_request;
            this.FIO = FIO;
            this.phone = phone;
            this.pk_subcat = pk_subcat;
            this.about = about;
            this.Date_req = Date_req;
            this.price = price;
        }


        public string makeSQLinsert()
        {
            string s1 = "insert into Request (FIO, phone, pk_subcat, price, about, Date_req) VALUES ('" + FIO + "', '" + phone + "', '" + pk_subcat + "', '" + price + "', '" + about + "', TO_DATE('" + Date_req + "','DD.MM.YYYY HH24:MI:SS'))";
            return s1;
        }

        public string makeSQLupdate()
        {
            string s1 = "update Request SET FIO = '" + FIO + "', pk_subcat='" + pk_subcat + "', price= '" + price + "', phone= '" + phone + "',about='" + about + "',Date_req=TO_DATE('" + Date_req + "','DD.MM.YYYY HH24:MI:SS') where pk_request= " + pk_request;
            return s1;
        }

        public string makeSQLdelete()
        {
            string s1 = "delete Request  where pk_request= " + pk_request;
            return s1;
        }
        //public Request(int pk_applicant, int pk_subcat, string about, DateTime Date_req)
        //{            
        //    this.pk_applicant = pk_applicant;
        //    this.pk_subcat = pk_subcat;
        //    this.about = about;
        //    this.Date_req = Date_req;
        //}




    }


    
}
