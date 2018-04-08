using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Education.DAL.Entities
{
    public class Ban : Entity
    {
        public string Reason { get; set; }
        public DateTime EndTime { get; set; } 
    }
}