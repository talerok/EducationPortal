using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum.Edit
{
    public class MessageEditDTO
    {
        public int Id { get; set; }
        public int ThemeId { get; set; }
        public string Text { get; set; }
    }
}
