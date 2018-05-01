using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum.Edit
{
    public class SectionEditDTO
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public bool Open { get; set; }
    }
}
