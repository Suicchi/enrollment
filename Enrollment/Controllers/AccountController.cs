using Enrollment.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Enrollment.Controllers
{
    public class AccountController : Controller
    {
        //To understand what I did go here https://www.youtube.com/watch?v=G6o9ilh6uBY
        private AccountRepository _repo;
        private AccountRepository Repo
        {
            get
            {
                if (_repo == null)
                {
                    _repo = new AccountRepository();
                }
                return _repo;
            }
        }

        //// GET: Account
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[HttpGet]
        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(Account account)
        {
            var adminAccount = Repo.Validate(account);
            ModelState.Clear();
            if (adminAccount != null)
            {
                Session["User"] = adminAccount;
                //Redirects to the Index action of Student controller in Admin area
                return RedirectToAction("Index", "Student", new { area = "Admin" });
            }
            else
            {

                ModelState.AddModelError("", "Username or Password Invalid");
                return View("Login");
            }

            //var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //var connection = new NpgsqlConnection(connectionString);
            //var query = "select * from \"Accounts\" where " +
            //    "username = '" + account.Username + "' and password = '" + account.Password + "' ";
            //var command = new NpgsqlCommand(query, connection);
            //connection.Open();
            //var reader = command.ExecuteReader();
            //if (reader.HasRows)
            //{
            //    connection.Close();
            //    //return RedirectToAction("Loggedin");
            //    return View("Loggedin");
            //}
            //else
            //{
            //    connection.Close();
            //    ModelState.AddModelError("", "Username or password wrong");
            //    return View("Login");
            //}
        }

        public ActionResult Logout()
        {
            Session.Clear();
            //or Session["User"] = null;
            return RedirectToAction("Login", "Account", new { area = "" });
        }
    }
}