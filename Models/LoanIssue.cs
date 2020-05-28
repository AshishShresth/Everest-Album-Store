using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverestAlbumStore.Models
{
    public class LoanIssue
    {
        public int AlbumId { get; set; }
        public int MemberId { get; set; }
        public DateTime IssuedDate { get; set; }
        public int count { get; set; }
    }
}