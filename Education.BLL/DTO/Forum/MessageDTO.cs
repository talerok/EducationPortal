using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public UserPublicInfoDTO Owner { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public UserPublicInfoDTO LastEditor { get; set; }
        public DateTime LastEditTime { get; set; }
    }
}
