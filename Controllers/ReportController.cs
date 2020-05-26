using EverestAlbumStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EverestAlbumStore.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        //Task1
        [Authorize(Roles ="Asistant, Manager")]
        public ActionResult FilterTitlesByLastName(string LastName)
        {
            ViewBag.LastName = db.Artists.ToList();
            if (string.IsNullOrEmpty(LastName))
            {
                return View();
            }
            var data = db.ArtistAlbums.Include("Albums").Include("Artist")
                .Where(x => x.Artist.LastName == LastName).ToList();
            return View(data);
        }

        //Task2
        public ActionResult FilterTitlesOnShelve(string LastName)
        {
            ViewBag.LastName = db.Artists.ToList();
            if(String.IsNullOrEmpty(LastName))
            {
                return View();
            }
            string Query = ("select * from Albums a" +
                " join Loans l ON l.AlbumId = a.Id" +
                " join ArtistAlbums aa ON aa.AlbumId = a.Id" +
                " join Artists ar on ar.Id = aa.ArtistId" +
                " where ar.LastName = '" + LastName + "' and ReturnedDate = '';");
            var data = db.Database.SqlQuery<Album>(Query);
            return View(data);
        }

        //Task3
        public ActionResult FilterMemberByIdOrLastname(string Name)
        {
            ViewBag.Name = db.Members.ToList();
            if (String.IsNullOrEmpty(Name))
            {
                return View();
            }
            string Query = ("Select * from Albums a"+
                " join ArtistAlbums aa on a.Id = aa.AlbumId"+
                " join Loans l on a.Id = l.AlbumId"+
                " join Members m on m.Id = l.MemberID"+
                " where m.name ='" + Name + "' and IssuedDate >= CURRENT_TIMESTAMP-31;");
            IEnumerable<Loan> data = db.Loans.SqlQuery(Query).ToList();

            //DateTime now = DateTime.Now;
            //var data = db.Loans.Include("Members").Include("AlbumStockDetail").Where(x => x.Members.Name == Name && DbFunctions.DiffDays(DateTime.Now, x.IssuedDate) >= 31).ToList();
            return View(data);
        }

        //Task 4
        public ActionResult ListAlbumIncreasingOrder(String Name)
        {
            var data = db.ArtistAlbums.Include("Albums").OrderBy(x => x.Albums.ReleasedDate).ToList();
            return View(data);
        }

        //Task 5
        public ActionResult SelectCopyByCopyNumber(int? id = null)
        {
            ViewBag.id = db.Albums.ToList();

            string Query = ("Select * From Loans l" +
                " join Albums a on a.Id = l.AlbumId" +
                " join Members m on m.Id = l.MemberId" +
                " where l.AlbumId = '"+ id +"' And IssuedDate = (Select Max (IssuedDate) From Loans where AlbumId = '"+ id +"');");
            IEnumerable<Loan> data = db.Loans.SqlQuery(Query).ToList();
            return View(data);
        }

        //Task 6

        //Task 7


        ////Task 8
        //public ActionResult AlphabeticalListOFMembers()
        //{
        //    var data = (from l in db.Loans
        //                where l.ReturnedDate == null group l by new { l.MemberId } into table1
        //                select new Loan
        //                {
                            

        //                })
        //}

        //Task 9

        
        //Task 10
        public ActionResult ListAllAlbumsAYearOld(String Title)
        {
            ViewBag.Title = db.Albums.ToList();
            string Query = ("Select * From Albums a" +
                " join Loans l on a.Id = l.AlbumId" +
                " where (CURRENT_TIMESTAMP-ReleasedDate)>365 AND ReturnedDate IS NOT NULL;");
            IEnumerable<Album> data = db.Albums.SqlQuery(Query).ToList();
            return View(data);
        }
        //Delete Albums older than a year for Task 10
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);

        }
        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
            db.SaveChanges();
            return RedirectToAction("ListAllAlbumsAYearOld");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Task 11

          
        //Task 12
        //Task13
        public ActionResult ListCopiesOfAlbumNotLoaned (string Title)
        {
            ViewBag.Title = db.Loans.ToList();
            string Query = ("Select * From Albums a"+
                " Join Loans l on l.AlbumId = a.Id"+
                " where IssuedDate <= (CURRENT_TIMESTAMP - 31);");
            IEnumerable<Loan> data = db.Loans.SqlQuery(Query).ToList();
            return View(data);
        }




    }
}