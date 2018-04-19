using Education.BLL.DTO;
using Education.BLL.Services.UserServices.Confirm;
using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Education.BLL.Services.UserServices.Restore
{
    
    public class RestorePasswordService : IRestorePasswordService
    {
        private IAuthKeyService emailService;
        private IAuthKeyService phoneService;
        private IUOWFactory DataFactory;
        private IRegValidator regValidator;
        private IPassHasher passHasher;
        public RestorePasswordService(IUOWFactory uowf, IAuthKeyService emailKeyService, IAuthKeyService phoneKeyService, IRegValidator reg, IPassHasher pHasher)
        {
            DataFactory = uowf;
            emailService = emailKeyService;
            phoneService = phoneKeyService;
            regValidator = reg;
            passHasher = pHasher;
        }

        private User GetUser(string login, IUOW Data)
        {
            return Data.UserRepository.Get().FirstOrDefault(x => 
                x.Login == login.ToLower()
                ||
                x.Phone != null && x.Phone.Confirmed && x.Phone.Value == login.ToLower()
                ||
                x.Email != null && x.Email.Confirmed && x.Email.Value == login.ToLower());
        }

        public RestoreResult Restore(string login, string password = null, string key = null)
        {
            using (var Data = DataFactory.Get())
            {
                var user = GetUser(login.ToLower(), Data);
                if (user == null) return new RestoreResult { Status = RestoreCode.UserNotFound };
                if (key == null)
                {
                    if (user.Phone != null && user.Phone.Confirmed)
                        return new RestoreResult { Status = RestoreCode.KeySent, SendTo = AuthType.Phone, KeyTime = phoneService.Generate(user.Phone,Data) };
                    else if (user.Email != null && user.Email.Confirmed)
                        return new RestoreResult { Status = RestoreCode.KeySent, SendTo = AuthType.Email, KeyTime = emailService.Generate(user.Email,Data) };
                    else return new RestoreResult { Status = RestoreCode.NeedContact };
                }

                KeyStatus status;

                if (user.Phone != null && user.Phone.Confirmed)
                    status = phoneService.Check(user.Phone, key, Data);
                else if (user.Email != null && user.Email.Confirmed)
                    status = emailService.Check(user.Email, key, Data);
                else return new RestoreResult { Status = RestoreCode.NeedContact };

                if (status == KeyStatus.Success)
                {
                    if (String.IsNullOrEmpty(password) || regValidator.checkPassword(password) != CheckResult.Ok)
                        return new RestoreResult { Status = RestoreCode.WrongPassword };

                    user.Password = passHasher.Get(password);
                    Data.UserRepository.Edited(user);
                    Data.SaveChanges();
                    return new RestoreResult { Status = RestoreCode.Succsess };
                }
                else if (status == KeyStatus.KeyTimeEnded)
                    return new RestoreResult { Status = RestoreCode.NeedNewKey };
                else return new RestoreResult { Status = RestoreCode.WorngKey };
            }
        }
    }
}
