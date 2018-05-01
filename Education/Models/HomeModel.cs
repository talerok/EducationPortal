using Education.BLL.DTO.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Models
{
    public class HomeModel
    {
        public IEnumerable<GroupPreviewDTO> Groups { get; set; }
        public bool CanCreateGroups { get; set; }
    }
}
