using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPDotNetMVC.Data;
using ASPDotNetMVC.Models;

namespace ASPDotNetMVC.Controllers
{
    
    public class ImportController : Controller
    {
        private ASPDotNetMVCContext db = new ASPDotNetMVCContext();
        // GET: Import
        public ActionResult Index()
        {
            return View();
        }
 
        /// <summary>
        /// Post method for importing users 
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(postedFile.FileName);
 
                    //Check uploaded file extension
                    if (fileExtension != ".csv")
                    {
                        ViewBag.Message = "Please select the csv file with .csv extension";
                        return View();
                    }
 
 
                    // Create list of Employees
                    var employees = new List<Employee>();
                    using (var sreader = new StreamReader(postedFile.InputStream))
                    {
                        // Header line.  
                        string[] headers = sreader.ReadLine()?.Split(',');
                        
                        // Loop though all records
                        while (!sreader.EndOfStream)
                        {
                            string[] rows = sreader.ReadLine()?.Split(',');

                            // If we have records in a file, add them
                            if (rows != null)
                                
                                employees.Add(new Employee
                                {
                                    Payroll_Number = rows[0].ToString(),
                                    Forename = rows[1].ToString(),
                                    Surname = rows[2].ToString(),
                                    Date_Of_Birth = rows[3].ToString(),
                                    PhoneNumber = rows[4].ToString(),
                                    MobileNumber = rows[5].ToString(),
                                    Address = rows[6].ToString(),
                                    Address_2 = rows[7].ToString(),
                                    Postcode = rows[8].ToString(),
                                    Email = rows[9].ToString(),
                                    Start_Date = rows[10].ToString(),
                                     
                                });
                        }
                    }

                    // Add model into database
                    foreach (var employee in employees)
                    {
                        db.Employees.Add(employee);
                        db.SaveChanges();
                    }
                    return View("View", employees);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "Please select the file first to upload.";
            }
            return View();
        }
    }
}