﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    class ThemePreviewDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserPublicInfoDTO Owner { get; set; }
        public MessageDTO LastMessages { get; set; }
    }
}
