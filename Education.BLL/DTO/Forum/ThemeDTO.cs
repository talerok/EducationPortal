﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    public class ThemeDTO
    {
        public int Id { get; set; }
        public bool Open { get; set; }
        public int SectionId { get; set; }
        public string Name { get; set; }
        public AccessDTO Access { get; set; }
        public IEnumerable<MessageDTO> Messages { get; set; }
    }
}
