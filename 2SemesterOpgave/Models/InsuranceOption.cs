using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class InsuranceOption
    {
        public string Name { get; set; }
        public float BaseFees { get; set; }
        public InsuranceOption()
        {
            Name = string.Empty;
            BaseFees = 0;
        }
    }
}
