using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Education.DAL.Entities
{
    public class UserInfo : Entity
    {
        public string FullName { get; set; }
        public string Avatar { get; set; }
    }
}
