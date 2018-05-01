using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Models
{
    public class SectionRequest
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public bool Open { get; set; }
    }
}
