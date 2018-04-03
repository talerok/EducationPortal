using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.UserServices.KeyGenerators
{
    public class BigKeyGenerator : IKeyGenerator
    {
        private static Random Random = new Random(05460546);
        private const short KeyLenght = 5;
        private string GeneratePart()
        {
            return Random.Next(10000, 99999).ToString();
        }

        private string GenerateKey()
        {
            string key = "";
            for(int i = 0; i < KeyLenght; i++)
            {
                if (i != 0) key += "-";
                key += GeneratePart();    
            }
            return key;
        }

        public Key Get()
        {
            return new Key
            {
                Value = GenerateKey(),
                EndTime = DateTime.Now.AddDays(1)
            };
        }
    }
}
