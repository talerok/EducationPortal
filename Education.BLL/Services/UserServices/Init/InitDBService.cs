using Education.BLL.Logic;
using Education.DAL.Interfaces;
using System.Linq;
using Education.DAL.Entities;
using Education.BLL.Services.UserServices.Auth;

namespace Education.BLL.Services.UserServices.Init
{
    public class InitDBService : Interfaces.IInitDBService
    {
        private IUOW Data;
        public InitDBService(IUOW data)
        {
            Data = data;
        }

        private void CheckAdmin(string login)
        {
            if (Data.UserRepository.Get().FirstOrDefault(x => x.Login == login || x.Level == 2) != null)
                throw new DBInitException("AdminAlreadyExists");
        }

        public void InitAdmin(string login, string password)
        {
            //-----------------------

            //-----------------------
            var hasher = new SHA256Hasher();
            CheckAdmin(login);
            Data.UserRepository.Add(new DAL.Entities.User
            {
                Login = login,
                Password = hasher.Get(password),
                Info = new DAL.Entities.UserInfo
                {
                    FullName = ""
                },
                Level = 2,
                Email = new Contact
                {
                    Value = "talerok@gmail.com",
                    Confirmed = true
                },
                Phone = new Contact
                {
                    Value = "+79995701322",
                    Confirmed = true
                },
                authType = AuthType.Phone
            });
        }
    }
}