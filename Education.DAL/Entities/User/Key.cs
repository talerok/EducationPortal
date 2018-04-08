using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Education.DAL.Entities
{
    public class Key : Entity
    {
        public string Value { get; set; }
        public DateTime EndTime { get; set; }
    }
}