using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace WikiRh.Models
{
    public class WikiRh_Question
    {
        public int Id_quest { get; set; }
        public string QuestionContent { get; set; }
        public string ResponsContent { get; set; }
        public int Count { get; set; }
        public string Url { get; set; }
        [ForeignKey("Id_cat")]
        [Column("Id_cat")]
        public int Id_cat { get; set; }
        public virtual WikiRh_Category Categories { get; set; }
        public List<WikiRh_File> Files { get; set; }

    }
}
