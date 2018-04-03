using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.DAL.Interfaces
{
    public interface IConfVal<T>
    {
        T Value { get; }
        bool Confirmed { get; }
    }
}
