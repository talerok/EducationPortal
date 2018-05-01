using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum.Edit
{
    public class GroupEditDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool Open { get; set; }
    }
}
