using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiRh.Models
{
   
    public class WikiRh_File
    {
        public int Id_file { get; set; }
        public string Name { get; set; }
      
        public byte[] Content { get; set; }

        public int Id_quest { get; set; }
        public virtual WikiRh_Question Questions { get; set; }

       
    }
}