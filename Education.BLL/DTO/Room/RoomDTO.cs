using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Room
{
    public class RoomDTO
    {
        public UserPublicInfoDTO Companion { get; set; }
        public IEnumerable<RoomMessageDTO> Messages { get; set; }
    }
}
