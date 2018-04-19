using Education.BLL.DTO.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Education.BLL.Services.ForumServices.Interfaces
{
    public enum ControlResult
    {
        succsess,
        notFound,
        noPremission,
        error
    }


    interface IControlService<T>
    {
        (ControlResult Code, T Value) Get(int Id, UserDTO userDTO);
        ControlResult Delete(int Id, UserDTO userDTO);
        ControlResult Update(int Id, T Value, UserDTO userDTO);
        (ControlResult Code, int Id) Create(int ParrentId, T Value, UserDTO userDTO);
    }
}