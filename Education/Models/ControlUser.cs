using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Models
{
    public enum Status
    {
        Request,
        Member,
        Owner,
        Delete
    }

    public class ControlUser
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public Status Status { get; set; }
    }
}
