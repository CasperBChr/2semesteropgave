using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Brand
    {
        public string Name { get; set; }    
        public string Description { get; set; }
        public string LogoPath { get; set; }
        public Brand(string name, string description, string logopath)
        {
            Name = name;
            Description = description;
            LogoPath = logopath;
        }
    }
}
