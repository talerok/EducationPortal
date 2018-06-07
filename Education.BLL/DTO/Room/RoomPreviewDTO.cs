using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Room
{
    public class RoomPreviewDTO
    {
        public UserPublicInfoDTO Companion { get; set; }
        public RoomMessageDTO LastMessage { get; set; }
    }
}
