using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.DAL.Repositories
{
    public class EFUOWFactory : IUOWFactory
    {
        private string cString;
        public EFUOWFactory(string connString)
        {
            cString = connString;
        }

        public IUOW Get()
        {
            return new EFUOW(cString);
        }
    }
}
