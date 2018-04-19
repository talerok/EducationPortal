﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    public class ThemeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Open { get; set; }
        public UserPublicInfoDTO Owner { get; set; }
        public MessageDTO Description { get; set; }
        public IEnumerable<MessageDTO> Messages { get; set; }
    }
}
