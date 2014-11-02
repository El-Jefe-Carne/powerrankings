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
    public class SportsController : Controller
    {
        private PowerRankingsEntities db = new PowerRankingsEntities();

        // GET: Sports
        public ActionResult Index()
        {
            //get the list of Sports which are not deleted
            return View(db.Sports.Where(x=>x.Deleted == false).ToList());
        }

        // GET: Sports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = db.Sports.Find(id);
            if (sport == null)
            {
                return HttpNotFound();
            }

            return View(sport);
        }

        // GET: Sports/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Sensitivity")] Sport sport)
        {
            if (ModelState.IsValid)
            {
                sport.Deleted = false;
                sport.CreateUser = User.Identity.Name;
                sport.CreateDate = DateTime.Now;

                db.Sports.Add(sport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sport);
        }

        // GET: Sports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = db.Sports.Find(id);
            if (sport == null)
            {
                return HttpNotFound();
            }
            return View(sport);
        }

        // POST: Sports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SportId,Name,Sensitivity")] Sport sport)
        {
            Sport sportToUpdate = db.Sports.Find(sport.SportId);

            if (sportToUpdate != null)
            {
                sportToUpdate.Name = sport.Name;
                sportToUpdate.Sensitivity = sport.Sensitivity;
                sportToUpdate.ModifyUser = User.Identity.Name;
                sportToUpdate.ModifyDate = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                db.Entry(sportToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sport);
        }

        // GET: Sports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = db.Sports.Find(id);
            if (sport == null)
            {
                return HttpNotFound();
            }
            return View(sport);
        }

        // POST: Sports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sport sport = db.Sports.Find(id);
            db.Sports.Remove(sport);
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
