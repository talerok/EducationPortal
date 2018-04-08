using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Education.DAL.Entities
{
    public class Contact : Entity
    {
        public string Value { get; set; }
        public bool Confirmed { get; set; }
        public virtual Key Key { get; set; }
        public virtual Key ConfirmKey { get; set; }
    }
}
