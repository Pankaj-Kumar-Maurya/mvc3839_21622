using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc3839_21622.Models
{
    public class TableCollection:tblemployee
    {
        public List<tblcountry> lstcountry { get; set; }
        public List<tblgender> lstgender { get; set; }
        public List<tblstate> lststate { get; set; }
        public List<tblhobby1> lsthobby1 { get; set; }
    }
}