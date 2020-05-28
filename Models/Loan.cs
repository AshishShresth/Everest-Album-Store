using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EverestAlbumStore.Models
{
    public class Loan
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime IssuedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        [NotMapped]
        public int LastLoanDays { get; set; }
        public int? Fine { get; set; }
        [Required]
        public int MemberId { get; set; }
        [Required]
        public int AlbumId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Members { get; set; }

        [ForeignKey("AlbumId")]
        public virtual Album Albums { get; set; }

        public virtual IEnumerable<Loan> Loans { get; set; }
    }

    public class LoanViewModel
    {
        [Key]
        public int id { get; set; }
        public int MemberId { get; set; }
        public string Name { get; set; }
        public string CName { get; set; }
        public int NumberOfLoans { get; set; }
        public string LoanStatus { get; set; }


    }
}