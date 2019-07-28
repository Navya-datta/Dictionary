using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcDictNew.Models
{
    public class DictModel
    {
        public List<string> Def { get; set; }
        public List<string> Syns { get; set; }
        public List<string> Ants { get; set; }
        public UsageClass Usage { get; set; }
    }

    public class UsageClass
    {
        public string Sample { get; set; }
    }

}