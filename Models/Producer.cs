using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EverestAlbumStore.Models
{
    public class Producer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Studio { get; set; }
        public virtual IEnumerable<Producer> Producers { get; set; }
    }
}