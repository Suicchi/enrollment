using Enrollment.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Enrollment.Models;

namespace Enrollment.Areas.Admin.Controllers
{
    public class StudentController : Controller
    {
        private StudentRepository _repo;
        private StudentRepository Repo
        {
            get
            {
                if(_repo == null)
                {
                    _repo = new StudentRepository();
                }
                return _repo;
            }
        }

        


        // GET: Admin/Student
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View("Index",Repo.GetAll());
        }

        public ActionResult Delete(int id)
        {
            Repo.Remove(id);
            ViewBag.SuccessMessage = "Item was successfully deleted";
            //Add this somewhere in the view that's logical
            //< p >@(ViewBag.SuccessMessage ?? "") </ p >
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Add()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            Student student = new Student();
            student.AdmissionDate = DateTime.Today;
            return View("Add", student);
        }

        [HttpPost]
        public ActionResult Add(Student student)
        {
            Repo.Add(student);
            ModelState.Clear();
            ViewBag.SuccessMessage = $"The information of {student.Name } was saved!" ;
            return View("Add");
        }

        //Edit pageAction
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            var student = Repo.Find(id);
            return View("Edit", student);
        }

        //Edit button action
        [HttpPost]
        public ActionResult Edit(Student student)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            Repo.Update(student);
            return RedirectToAction("Details", new { id = student.ID });
        }

        //Does it's job when user clicks details button
        public ActionResult Details(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            var student = Repo.Find(id);
            return View("Details", student);
        }

        //Gives the search result
        [HttpPost]
        public ActionResult Search(string entryToFind)
        {
            int id;
            if(int.TryParse(entryToFind, out id))
            {
                var student = Repo.FindAsList(id);
                if (student == null)
                {
                    ViewBag.ErrorMessage = "Not found";
                    return View("Index", student);
                }
                else
                {
                    return View("Index", student);
                }
            }
            else
            {
                var student = Repo.FindByName(entryToFind);
                if(student == null)
                {
                    //Need some kind of error message to return with view
                    ViewBag.Message = "Not found";
                    return View("Index");
                }
                else
                {
                    return View("Index", student);
                }


            }
            
        }

        public ActionResult About()
        {
            if(Session["User"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View("About");
        }

    }
}