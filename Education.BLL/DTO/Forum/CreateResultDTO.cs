using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    public enum AccessCode
    {
        Succsess,
        NoPremision,
        NotFound,
        Error
    }

    public class CreateResultDTO
    {
        public static CreateResultDTO NoPremision
        {
            get
            {
                return new CreateResultDTO(-1, AccessCode.NoPremision);
            }
        }

        public static CreateResultDTO NotFound
        {
            get
            {
                return new CreateResultDTO(-2, AccessCode.NotFound);
            }
        }

        public static CreateResultDTO Error
        {
            get
            {
                return new CreateResultDTO(-3, AccessCode.Error);
            }
        }

        public CreateResultDTO(int id, AccessCode code)
        {
            Id = id;
            Code = code;
        }
        public int Id { get; set; }
        public AccessCode Code { get; set; }
    }
}
