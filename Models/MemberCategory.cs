using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EverestAlbumStore.Models
{
    public class MemberCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int TotalLoan { get; set; }
        [Required]
        public int ReturningDays { get; set; }
        public virtual IEnumerable<MemberCategory> MemberCategories { get; set; }
    }
}