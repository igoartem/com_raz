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
        public int pk_applicant { get; set; }
        public int pk_subcat { get; set; }
        public string about { get; set; }
        public DateTime Date_req { get; set; }

        //Убрать пк_апликант, добавить ФИО и ТЕлефон

        public Request(int pk_request, int pk_applicant, int pk_subcat, string about, DateTime Date_req)
        {
            this.pk_request = pk_request;
            this.pk_applicant = pk_applicant;
            this.pk_subcat = pk_subcat;
            this.about = about;
            this.Date_req = Date_req;
        }

        public Request(int pk_applicant, int pk_subcat, string about, DateTime Date_req)
        {            
            this.pk_applicant = pk_applicant;
            this.pk_subcat = pk_subcat;
            this.about = about;
            this.Date_req = Date_req;
        }




    }


    
}
