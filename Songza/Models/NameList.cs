using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Songza.Models
{
    public class NameList
    {
        public string name { get; set; }
        public NameList(string name)
        {
            this.name = name;
        }
    }
}