using Education.BLL.DTO.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Models
{
    public class PageControl
    {
        public IEnumerable<PageInfo> Map { get; set; }
        public PageDTO PageDTO { get; set; } 
    }
}
