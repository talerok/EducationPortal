using System;
using System.Collections.Generic;
using System.Text;

namespace Education.DAL.Interfaces
{
    public interface IUOWFactory 
    {
        IUOW Get();
    }
}
