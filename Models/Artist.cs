using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EverestAlbumStore.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthOfDate { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNmber { get; set; }

        public virtual IEnumerable<Artist> Artists { get; set; }
    }
}