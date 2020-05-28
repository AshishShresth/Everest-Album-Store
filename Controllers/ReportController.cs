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
            var loans = db.Loans.Include(l => l.Albums).Include(l => l.Members);
            return View(loans.ToList());
        }

        //Task1
        [AllowAnonymous]
        
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
        [AllowAnonymous]
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
        [Authorize(Roles = "Manager, Assistant")]
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
        [Authorize(Roles = "Manager, Assistant")]
        public ActionResult ListAlbumIncreasingOrder(String Name)
        {
            var data = db.ArtistAlbums.Include("Albums").OrderBy(x => x.Albums.ReleasedDate).ToList();
            return View(data);
        }

        //Task 5
        [Authorize(Roles = "Manager, Assistant")]
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
        [Authorize(Roles = "Manager, Assistant")]
        public ActionResult AlbumCopyLoanToMember()
        {
            ViewBag.AlbumId = new SelectList(db.Albums, "Id", "Tittle");
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlbumCopyLoanToMember([Bind(Include = "Id,AlbumId,IssuedDate,ReturnedDate,MemberId")]Loan loan)
        {
            if(ModelState.IsValid)
            {
                ViewBag.AlbumId = new SelectList(db.Albums, "Id", "Tittle", loan.AlbumId);
                ViewBag.MemberId = new SelectList(db.Members, "Id", "Name", loan.MemberId);
                var data = loan.AlbumId;
                var member = loan.MemberId;
                Album album = db.Albums.Find(data);
                int memberId = member;
                int albumId = data;
                CheckAlbumStock(albumId);
                string ageVerify = album.AgeRestriction;
                if(CheckNumberOfLoan(memberId) == true)
                {
                    if(CheckAlbumStock(albumId) == true)
                    {
                        if(ageVerify == "18+")
                        {
                            if(CheckAge(memberId) == true)
                            {
                                if(data != null)
                                {
                                    int copyNumber = album.CopyNumber;
                                    int a = copyNumber - 1;
                                    album.CopyNumber = copyNumber;
                                    db.Entry(album).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                db.Loans.Add(loan);
                                db.SaveChanges();
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Sorry, This Album is Age Restricted, You Must be 18 Years or Older";
                                return View();
                            }

                        }
                        else
                        {
                            if(data != null)
                            {
                                int copyNumber = album.CopyNumber;
                                int a = copyNumber - 1;
                                album.CopyNumber = copyNumber;
                                db.Entry(album).State = EntityState.Modified;
                                db.SaveChanges();
                               
                            }
                            db.Loans.Add(loan);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Sorry, The Album is Out Of Stock";
                        return View();
                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "Album Loan has been out of date";
                    return View();
                }
            }
            return View(loan);
        }

        private bool CheckAge(int memberID)
        {
            Member member = db.Members.Find(memberID);
            DateTime dateOfBirth = member.DateOfBirth;
            DateTime currentDate = DateTime.Today;
            TimeSpan timeSpan = currentDate.Subtract(dateOfBirth);
            int years = timeSpan.Days / 365;
            if (years >= 18)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //public Boolean CheckAge(int memberID)
        //{
        //    Member member = db.Members.Find(memberID);
        //    DateTime dateOfBirth = member.DateOfBirth;
        //    DateTime currentDate = DateTime.Today;
        //    TimeSpan timeSpan = currentDate.Subtract(dateOfBirth);
        //    int years = timeSpan.Days / 365;
        //    if (years >= 18)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public Boolean CheckNumberOfLoan(int memberID)
        {
            Member member = db.Members.Find(memberID);
            string memberName = member.Name;
            int memberCatId = member.MemberCategoryId;
            MemberCategory memberCategory = db.MemberCategories.Find(memberCatId);
            int? loan = memberCategory.TotalLoan;
            var list = db.Loans.Include("Members").Where(x => x.Members.Name == memberName).Count();
            if(list > loan)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public Boolean CheckAlbumStock(int Id)
        {
            Album album = db.Albums.Find(Id);
            int copyNumber = album.CopyNumber;
            if(copyNumber > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Task 7
        [Authorize(Roles = "Manager, Assistant")]
        public ActionResult RecordReturnOfAlbumCopy(string Name)
        {
            ViewBag.Name = db.Loans.ToList();
            string Query = ("Select * From Loans Where ReturnedDate is null;");
            var data = db.Loans.SqlQuery(Query).ToList();
            return View(data);
        }
        // GET: Loans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumId = new SelectList(db.Albums, "Id", "Tittle", loan.AlbumId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Name", loan.MemberId);
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IssuedDate,ReturnedDate,MemberId,AlbumId")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RecordReturnOfAlbumCopy");
            }
            ViewBag.AlbumId = new SelectList(db.Albums, "Id", "Tittle", loan.AlbumId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Name", loan.MemberId);
            return View(loan);
        }


        //Task 8

        //public ActionResult AlphabeticalListOFMembers()
        //{
        //    var data = (from l in db.Loans
        //                where l.ReturnedDate == null group l by new { l.MemberId } into table1
        //                select new Loan
        //                {


        //                })
        //}

        //Task 8
        [Authorize(Roles = "Manager, Assistant")]
        public ActionResult AlphabeticListOfAllMembers()
        {
            var data = (from l in db.Loans
                        group l by new { l.MemberId } into table1
                        select new
                        {
                            NumberOfLoans = table1.Count(),
                            MemberId = table1.Key.MemberId,
                        }).ToList().Select(x => new LoanViewModel()
                        {
                            NumberOfLoans = x.NumberOfLoans,
                            MemberId = x.MemberId
                        });
            var d = (from l in data
                     join m in db.Members on
                     l.MemberId equals m.Id
                     join mc in db.MemberCategories on
                     m.MemberCategoryId equals mc.Id
                     select new
                     {
                         MemberId = l.MemberId,
                         Name = m.Name,
                         CName = mc.CategoryName,
                         NumberOfLoans = l.NumberOfLoans,
                         LoanStatus = (l.NumberOfLoans > mc.TotalLoan ? "Too Many Albums" : "Number Of Loans Satisfies the Membership Category")
                     }
                     ).ToList().Select(x => new LoanViewModel
                     {
                         MemberId = x.MemberId,
                         Name = x.Name,
                         CName = x.CName,
                         NumberOfLoans = x.NumberOfLoans,
                         LoanStatus = x.LoanStatus
                     });
            var dd = d.OrderBy(x => x.Name);
            return View(dd);         
        }



        //Task 9


        //Task 10
        [Authorize(Roles = "Manager, Assistant")]
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
        [Authorize(Roles = "Manager, Assistant")]
        public ActionResult ListCopiesOfAlbumsOnLoan()
        {
            List<Member> members = db.Members.ToList();
            List<Album> albums = db.Albums.ToList();
            List<Loan> loans = db.Loans.ToList();
            List<TotalLoan> totalLoans = loans
                .Where(a => a.ReturnedDate == null)
                .GroupBy(l => l.IssuedDate)
                .Select(al => new TotalLoan
                {
                    IssuedDate = al.Select(x => x.IssuedDate).FirstOrDefault(),
                    count = al.Count(),
                }).ToList();
            List<LoanIssue> loanIssues = (from l in loans
                                          join tl in totalLoans on l.IssuedDate equals tl.IssuedDate
                                          select new LoanIssue
                                          {
                                              MemberId = l.MemberId,
                                              AlbumId = l.AlbumId,
                                              IssuedDate = l.IssuedDate,
                                              count = tl.count,
                                          }).ToList();
            var result = from m in members
                         join li in loanIssues on m.Id equals li.MemberId
                         join a in albums on li.AlbumId equals a.Id
                         orderby li.IssuedDate
                         select new AlbumLoaned
                         {
                             member = m,
                             album = a,
                             rent = li,
                         };
            return View(result);
        }

        //Task 12
        [Authorize(Roles = "Manager, Assistant")]
        public ActionResult ListOfMembersAlbumNotBorrowed(string Name)
        {
            ViewBag.Name = db.Loans.ToList();
            string Query = ("Select * From members m " +
                "join loans l on l.memberid = m.id " +
                "join albums a on a.id = l.albumid " +
                "where issueddate <= (current_timestamp - 31) " +
                "and datediff(day, issueddate, current_timestamp) " +
                "in (select min(datediff(day, issueddate, current_timestamp)) from loans where (datediff(day, issueddate, current_timestamp))>31) " +
                "order by issueddate asc");

            IEnumerable<Loan> data = db.Loans.SqlQuery(Query).ToList();
            return View(data);
        }
        //Task13
        [Authorize(Roles = "Manager, Assistant")]
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