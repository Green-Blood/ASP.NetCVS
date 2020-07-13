using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPDotNetMVC.Data;
using ASPDotNetMVC.Models;
using PagedList;

namespace ASPDotNetMVC.Controllers
{
    public class EmployeesController : Controller
    {
        private ASPDotNetMVCContext db = new ASPDotNetMVCContext();

        // GET: Employees
        public ViewResult Index(string sortOrder, string currentFilter,  string searchString, int ?page)
        {
            // Get data from Database
            var employees = from e in db.Employees
                select e;

            #region Sorting
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm  = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.ForeNameParm = sortOrder == "Forename" ? "Forename_desc" : "Forename";
            
            // Sort
            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.Surname);
                    break;
                case "Date":
                    employees = employees.OrderBy(e => e.Start_Date);
                    break;
                case "date_desc":
                    employees = employees.OrderByDescending(e => e.Start_Date);
                    break;
                case "Forename":
                    employees = employees.OrderBy(e => e.Forename);
                    break;
                case "Forename_desc":
                    employees = employees.OrderByDescending(e => e.Forename);
                    break;
                default:
                    employees = employees.OrderBy(s => s.Surname);
                    break;
            }
            #endregion

            #region Search
            // Check if we have searched already or not 
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                // If we searched shows filter
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            // Search 
            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.Forename.Contains(searchString)
                                                 || e.Forename.Contains(searchString));
            }
            
            #endregion
            
            // How many datum can be in one page(Could be changed to give dynamic page data like 5,10,15,20 and etc.)
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            

            return View( employees.ToPagedList(pageNumber,  pageSize));
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Payroll_Number,Forename,Surname,Date_Of_Birth,PhoneNumber,MobileNumber,Address,Address_2,Postcode,Email,Start_Date")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Payroll_Number,Forename,Surname,Date_Of_Birth,PhoneNumber,MobileNumber,Address,Address_2,Postcode,Email,Start_Date")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
