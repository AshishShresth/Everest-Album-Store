using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EverestAlbumStore.Models
{
    public class ArtistAlbum
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ArtistId { get; set; }
        [Required]
        public int AlbumId { get; set; }
        [Required]
        public int ProducerId { get; set; }

        [ForeignKey("ArtistId")]
        public virtual Artist Artist { get; set; }

        [ForeignKey("AlbumId")]
        public virtual Album Albums { get; set; }

        [ForeignKey("ProducerId")]
        public virtual Producer Producers { get; set; }

        public virtual IEnumerable<ArtistAlbum> ArtistAlbums { get; set; }
    }
}