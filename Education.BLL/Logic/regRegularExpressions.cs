using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic
{
    public static class regRegularExpressions
    {
        public static string EmailRegex = @".+@.+\..+";
        public static string LoginRegex = @"^[\w\d\-]{4,20}$";
        public static string PhoneRegex = @"^\+?\d{11}$";
        public static string PasswordRegex = @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,20})";
        public static string FullNameRegex = @"^[А-Я][а-я]* [А-Я][а-я]* [А-Я][а-я]*$";
    }
}
