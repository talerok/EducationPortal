using Education.DAL.Interfaces;

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
