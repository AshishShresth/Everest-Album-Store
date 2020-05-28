using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverestAlbumStore.Models
{
    public class AlbumLoaned
    {
        public Member member { get; set; }
        public Album album { get; set; }
        public LoanIssue rent { get; set; }
    }
}