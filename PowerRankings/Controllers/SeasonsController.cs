using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PowerRankings.Models;

namespace PowerRankings.Controllers
{
    [Authorize]
    public class SeasonsController : Controller
    {
        private PowerRankingsEntities db = new PowerRankingsEntities();

        // GET: Seasons
        public ActionResult Index()
        {
            var seasons = db.Seasons.Include(s => s.Sport).Where(x=>!x.Deleted);
            return View(seasons.ToList());
        }       

        // GET: Seasons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Season season = db.Seasons.Find(id);
            if (season == null)
            {
                return HttpNotFound();
            }            

            return View(season);
        }

        // GET: Seasons/Create
        public ActionResult Create()
        {
            ViewBag.SportId = new SelectList(db.Sports, "SportId", "Name");
            return View();
        }

        // POST: Seasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SportId,Name,Description,Active")] Season season)
        {
            season.CreateDate = DateTime.Now;
            season.CreateUser = User.Identity.Name;
            season.Deleted = false;
            season.Active = true;

            if (ModelState.IsValid)
            {
                db.Seasons.Add(season);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SportId = new SelectList(db.Sports, "SportId", "Name", season.SportId);
            return View(season);
        }

        // GET: Seasons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Season season = db.Seasons.Find(id);
            if (season == null)
            {
                return HttpNotFound();
            }
            ViewBag.SportId = new SelectList(db.Sports, "SportId", "Name", season.SportId);
            return View(season);
        }

        // POST: Seasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SeasonId,SportId,Name,Description,CreateUser,CreateDate,ModifyUser,ModifyDate,Deleted,Active")] Season season)
        {
            if (ModelState.IsValid)
            {
                db.Entry(season).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SportId = new SelectList(db.Sports, "SportId", "Name", season.SportId);
            return View(season);
        }

        // GET: Seasons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Season season = db.Seasons.Find(id);
            if (season == null)
            {
                return HttpNotFound();
            }
            return View(season);
        }

        // POST: Seasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Season season = db.Seasons.Find(id);
            db.Seasons.Remove(season);
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
