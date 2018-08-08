using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Website;

/*
 * Author: Brian Tat
 * Description: API Controller class for API
 * Available Custom API routes for this version are:
 * - api/accounts/user={name}               
 * - api/accounts/user={name}|pass={password}
 * - api/accounts/{id}/update
 */

namespace Website.Controllers
{
    public class AccountsController : ApiController
    {
        //MazeGame Entities with the base class of DbContext for connecting to db for creating, inserting and deleting
        private AMazeGameEntities1 db = new AMazeGameEntities1();

        // GET: api/Accounts
        public IQueryable<Account> GetAccounts()
        {
            DbSet<Account> accounts = db.Accounts;

            foreach (Account a in accounts)
            {
              a.Password = "";
            }

            return accounts;
        }

        
        // GET: api/Accounts/5
        [ResponseType(typeof(Account))]
        public IHttpActionResult GetAccount(int id)
        {
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }
            account.Password="";
            return Ok(account);
        }
         

        //Get: api/Accounts/user=5
        [Route("api/accounts/user={username}")]
        [ResponseType(typeof(Account))]
        public IHttpActionResult GetAccountByName(string username)
        {
          Account account = db.Accounts.Where(a => a.Username.ToUpper() == username.ToUpper()).FirstOrDefault();
          if (account == null)
          {
            return NotFound();
          }
          account.Password="";
          return Ok(account);
        }

        [Route("api/accounts/user={username}|pass={password}")]
        [ResponseType(typeof(Account))]
        public IHttpActionResult GetAccountByUserNamePassword(string username, string password)
        {
          Account account = db.Accounts.Where(a => a.Username.ToUpper() == username.ToUpper() &&  a.Password == password).FirstOrDefault();
          if (account == null)
          {
            return NotFound();
          }
          account.Password = "";
          return Ok(account);
        }

        // PUT: api/Accounts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAccount(int id, Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != account.Id)
            {
                return BadRequest();
            }

            db.Entry(account).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/accounts/{id}/update")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateAccount(int id, Account account)
        {
          if (!ModelState.IsValid)
          {
            return BadRequest(ModelState);
          }

          
          if (id != account.Id)
          {
            return BadRequest();
          }
           

          var v = db.Accounts.Find(id);
          account.Password = db.Accounts.Find(account.Id).Password;
          account.Username = db.Accounts.Find(account.Id).Username;

          //db.Entry(account).State = EntityState.Modified;
          db.Entry(v).CurrentValues.SetValues(account);

          try
          {
            db.SaveChanges();
          }
          catch (DbUpdateConcurrencyException)
          {
            if (!AccountExists(id))
            {
              return NotFound();
            }
            else
            {
              throw;
            }
          }

          return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Accounts
        [ResponseType(typeof(Account))]
        public IHttpActionResult PostAccount(Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!AccountNameExist(account.Username))
            {
              db.Accounts.Add(account);
              db.SaveChanges();
            }

            return CreatedAtRoute("DefaultApi", new { id = account.Id }, account);
        }

        // DELETE: api/Accounts/5
        [ResponseType(typeof(Account))]
        public IHttpActionResult DeleteAccount(int id)
        {
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }

            db.Accounts.Remove(account);
            db.SaveChanges();

            return Ok(account);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AccountExists(int id)
        {
            return db.Accounts.Count(e => e.Id == id) > 0;
        }

        private bool AccountNameExist(string username)
        {
          return db.Accounts.Count(e => e.Username.Equals(username)) > 0;
        }
    }
}