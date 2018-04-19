using Education.BLL.DTO.Forum;
using Education.BLL.DTO.User;
using Education.BLL.Services.ForumServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.ForumServices
{
    class ThemeService : IControlService<ThemeDTO>
    {
        public (ControlResult Code, int Id) Create(int ParrentId, ThemeDTO Value, UserDTO userDTO)
        {
            throw new NotImplementedException();
        }

        public ControlResult Delete(int Id, UserDTO userDTO)
        {
            throw new NotImplementedException();
        }

        public (ControlResult Code, ThemeDTO Value) Get(int Id, UserDTO userDTO)
        {
            throw new NotImplementedException();
        }

        public ControlResult Update(int Id, ThemeDTO Value, UserDTO userDTO)
        {
            throw new NotImplementedException();
        }
    }
}
