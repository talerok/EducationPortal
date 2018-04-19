﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    class GroupDTO
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool Open { get; set; }
        public IEnumerable<SectionDTO> Sections { get; set; }
    }
}
