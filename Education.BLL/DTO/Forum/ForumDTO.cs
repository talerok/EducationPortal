using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    public class ForumDTO
    {
        public bool CanCreateGroup { get; set; }
        public IEnumerable<GroupPreviewDTO> Groups { get; set; }
    }
}
