using System;
using System.Collections.Generic;
using System.Text;
using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Entities;

namespace Education.BLL.Services.UserServices.KeyGenerators
{
    public class SmallKeyGenerator : IKeyGenerator
    {
        private static Random Random = new Random(05470547);

        public SmallKeyGenerator()
        {

        }

        public Key Get()
        {
            return new Key
            {
                Value = Random.Next(10000, 99999).ToString(),
                EndTime = DateTime.Now.AddMinutes(5)
            };
        }
    }
}
