using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Pages
{
    public class NoteDTO : NoteEditDTO
    {
        public DateTime Time { get; set; }
        public UserPublicInfoDTO Owner { get; set; }
        public AccessDTO Access { get; set; }

    }
}
