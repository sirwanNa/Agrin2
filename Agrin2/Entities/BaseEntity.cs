using Agrin2.Generals;
using System;
using System.ComponentModel.DataAnnotations;

namespace Agrin2.Entities
{
    public class BaseEntity
    {
        //public BaseEntity()
        //{           
        //    IsDeleted = false;
        //    Id = Utilities.CreateGuid();
        //}
        [Key]
        public Guid Id { get; set; }      
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }        
    }
}
