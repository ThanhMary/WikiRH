using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiRh.Models
{
    public class WikiRh_Category
    {
        public int Id_cat { get; set; }
        public string Name { get; set; }
        public List<WikiRh_Question> Questions { get; set; }

     
    }
}
