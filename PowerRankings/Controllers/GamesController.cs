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
    public class GamesController : Controller
    {
        private PowerRankingsEntities db = new PowerRankingsEntities();

        // GET: Games
        public ActionResult Index()
        {
            var games = db.Games.Include(g => g.Team).Include(g => g.Team1).Include(g => g.Season);
            return View(games.ToList());
        }

        // GET: Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: Games/Create
        public ActionResult Create(int? id)
        {
            Game game = new Game();
            if (id != null)            
            {
                game.Season = db.Seasons.Find((int)id);
            }           

            ViewBag.Team1Id = new SelectList(db.Teams, "TeamId", "Name");
            ViewBag.Team2Id = new SelectList(db.Teams, "TeamId", "Name");            
            return View(game);
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SeasonId,Team1Id,Team2Id,Team1Score,Team2Score,EventDate,Location,Comment,Forfeited,Included")] Game game, string submit)
        {
            game.CreateDate = DateTime.Now;
            game.CreateUser = User.Identity.Name;
            game.Deleted = false;
            
            //forfeited games are not included in the rankings
            if (game.Forfeited)
                game.Included = false;
                        
            if (ModelState.IsValid)
            {
                db.Games.Add(game);
                db.SaveChanges();

                if (submit == "Create")
                {
                    return RedirectToAction("Details", "Seasons", new { id = game.SeasonId });    
                }
                else
                {
                    return RedirectToAction("Create", "Games", new { id = game.SeasonId });
                }                
            }

            ViewBag.Team1Id = new SelectList(db.Teams, "TeamId", "Name", game.Team1Id);
            ViewBag.Team2Id = new SelectList(db.Teams, "TeamId", "Name", game.Team2Id);            
            return View(game);
        }

        // GET: Games/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.Team1Id = new SelectList(db.Teams, "TeamId", "CreateUser", game.Team1Id);
            ViewBag.Team2Id = new SelectList(db.Teams, "TeamId", "CreateUser", game.Team2Id);
            ViewBag.SeasonId = new SelectList(db.Seasons, "SeasonId", "Name", game.SeasonId);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GameId,SeasonId,Team1Id,Team2Id,Team1Score,Team2Score,EventDate,Location,Comment,Included,CreateUser,CreateDate,ModifyUser,ModifyDate,Forfeited,Deleted")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Team1Id = new SelectList(db.Teams, "TeamId", "CreateUser", game.Team1Id);
            ViewBag.Team2Id = new SelectList(db.Teams, "TeamId", "CreateUser", game.Team2Id);
            ViewBag.SeasonId = new SelectList(db.Seasons, "SeasonId", "Name", game.SeasonId);
            return View(game);
        }

        // GET: Games/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
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
