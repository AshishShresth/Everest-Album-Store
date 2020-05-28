using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EverestAlbumStore.Models;

namespace EverestAlbumStore.Controllers
{
    public class ArtistAlbumsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ArtistAlbums
        [Authorize(Roles = "Manager, Assistant")]
        public ActionResult Index()
        {
            var artistAlbums = db.ArtistAlbums.Include(a => a.Albums).Include(a => a.Artist).Include(a => a.Producers);
            return View(artistAlbums.ToList());
        }

        // GET: ArtistAlbums/Details/5
        [Authorize(Roles = "Manager, Assistant")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtistAlbum artistAlbum = db.ArtistAlbums.Find(id);
            if (artistAlbum == null)
            {
                return HttpNotFound();
            }
            return View(artistAlbum);
        }

        // GET: ArtistAlbums/Create
        [Authorize(Roles = "Manager, Assistant")]
        public ActionResult Create()
        {
            ViewBag.AlbumId = new SelectList(db.Albums, "Id", "Tittle");
            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "FirstName");
            ViewBag.ProducerId = new SelectList(db.Producers, "Id", "Name");
            return View();
        }

        // POST: ArtistAlbums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ArtistId,AlbumId,ProducerId")] ArtistAlbum artistAlbum)
        {
            if (ModelState.IsValid)
            {
                db.ArtistAlbums.Add(artistAlbum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AlbumId = new SelectList(db.Albums, "Id", "Tittle", artistAlbum.AlbumId);
            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "FirstName", artistAlbum.ArtistId);
            ViewBag.ProducerId = new SelectList(db.Producers, "Id", "Name", artistAlbum.ProducerId);
            return View(artistAlbum);
        }

        // GET: ArtistAlbums/Edit/5
        [Authorize(Roles = "Manager, Assistant")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtistAlbum artistAlbum = db.ArtistAlbums.Find(id);
            if (artistAlbum == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumId = new SelectList(db.Albums, "Id", "Tittle", artistAlbum.AlbumId);
            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "FirstName", artistAlbum.ArtistId);
            ViewBag.ProducerId = new SelectList(db.Producers, "Id", "Name", artistAlbum.ProducerId);
            return View(artistAlbum);
        }

        // POST: ArtistAlbums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ArtistId,AlbumId,ProducerId")] ArtistAlbum artistAlbum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artistAlbum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AlbumId = new SelectList(db.Albums, "Id", "Tittle", artistAlbum.AlbumId);
            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "FirstName", artistAlbum.ArtistId);
            ViewBag.ProducerId = new SelectList(db.Producers, "Id", "Name", artistAlbum.ProducerId);
            return View(artistAlbum);
        }

        // GET: ArtistAlbums/Delete/5
        [Authorize(Roles = "Manager, Assistant")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtistAlbum artistAlbum = db.ArtistAlbums.Find(id);
            if (artistAlbum == null)
            {
                return HttpNotFound();
            }
            return View(artistAlbum);
        }

        // POST: ArtistAlbums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArtistAlbum artistAlbum = db.ArtistAlbums.Find(id);
            db.ArtistAlbums.Remove(artistAlbum);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
