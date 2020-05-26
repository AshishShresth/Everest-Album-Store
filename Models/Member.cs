using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EverestAlbumStore.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public int MemberCategoryId { get; set; }

        [ForeignKey("MemberCategoryId")]
        public virtual MemberCategory MemberCategories { get; set; }
        public virtual IEnumerable<Member> Members { get; set; }
    }
}