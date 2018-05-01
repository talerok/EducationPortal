using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    public class MessageRoute
    {
        public int ThemeId { get; set; }
        public string ThemeName { get; set; }
        public ThemeRoute ThemeRoute { get; set; }
    }


    public class MessageDTO
    {
        public int Id { get; set; }
        public UserPublicInfoDTO Owner { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public UserPublicInfoDTO LastEditor { get; set; }
        public DateTime LastEditTime { get; set; }
        public AccessDTO Access { get; set; }
        public MessageRoute Route { get; set; }
    }
}
