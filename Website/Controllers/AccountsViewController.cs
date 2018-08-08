using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Website;
using System.Net.Http;

/*
 * Author: Brian Tat
 * Description: Controller used to display Account information using the API Controller
 * Available Custom API routes for this version are:
 * - api/accounts/user={name}               
 * - api/accounts/user={name}|pass={password}
 * - api/accounts/{id}/update
 */

namespace Website.Controllers
{
    public class AccountsViewController : Controller
    {
        private AMazeGameEntities1 db = new AMazeGameEntities1();
        private string webAddress = "http://192.168.0.7";

        // GET: AccountsMVC
        public ActionResult Index()
        {
            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri(webAddress);

            HttpResponseMessage hrm;

            hrm = hc.GetAsync("api/accounts").Result;

            List<Account> accounts = hrm.Content.ReadAsAsync<List<Account>>().Result;

            return View(accounts);
        }

        // GET: AccountsMVC/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
              return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri(webAddress);
            HttpResponseMessage hrm;
            hrm = hc.GetAsync("api/accounts/" + id).Result;
            Account account = hrm.Content.ReadAsAsync<Account>().Result;
            return View(account);
        }

        // GET: AccountsMVC/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountsMVC/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,GamesPlayed,GamesWon,Kills,Deaths,ItemsUsed,PuzzlesCompleted")] Account account)
        {
          HttpClient hc = new HttpClient();
          hc.BaseAddress = new Uri(webAddress);
          var postData = new List<KeyValuePair<string, string>>();
          postData.Add(new KeyValuePair<string,string>("Username", account.Username));
          postData.Add(new KeyValuePair<string, string>("Password", account.Password));

          HttpContent content = new FormUrlEncodedContent(postData);
          HttpResponseMessage hrm;
          hrm = hc.PostAsync("api/accounts/", content).Result;
         if (hrm.Content.ReadAsAsync<Account>().Result == null)
          {
             return View(account);
          }
            /*if (ModelState.IsValid)
            {
                HttpClient hc = new HttpClient();
                hc.BaseAddress = new Uri(webAddress);
                HttpContent hcon = 
                hc.PostAsync<Account>("api/accounts/", account);
                //db.Accounts.Add(account);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            */
           return RedirectToAction("Index");
           //return View(account);
        }

        // GET: AccountsMVC/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: AccountsMVC/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,GamesPlayed,GamesWon,Kills,Deaths,ItemsUsed,PuzzlesCompleted")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: AccountsMVC/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: AccountsMVC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
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
