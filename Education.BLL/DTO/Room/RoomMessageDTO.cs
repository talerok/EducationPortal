using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Room
{
    public class RoomMessageDTO
    {
        public int Id { get; set; }
        public UserPublicInfoDTO From { get; set; }
        public UserPublicInfoDTO To { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }
}
