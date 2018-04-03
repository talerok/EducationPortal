using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Education.BLL.Services.UserServices.Auth
{
    public class SHA256Hasher : Interfaces.IPassHasher
    {
        SHA256 SHA256 = new SHA256Managed();

        public string Get(string input)
        {
            if (input == null) return null;
            var bytes = Encoding.Default.GetBytes(input);
            var hashbytes = SHA256.ComputeHash(bytes);
            return BitConverter.ToString(hashbytes).ToLower();
        }
    }
}