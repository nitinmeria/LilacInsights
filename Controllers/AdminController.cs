using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using TargetData.NHibernate;
using NHibernate;
using LilacInsights.Model;
using LilacInsights.DataAccess;
using PagedList;

namespace LilacInsight.Controllers
{
    public class AdminController : Controller
    {
               
        // GET: /Admin/
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["error"] = TempData["error"];
            return View();
        }

        [HttpPost]
        
        public ActionResult Index(Refmaster rfobj)
        {
            LilacDao getBool = new LilacDao(ConfigurationManager.ConnectionStrings["lilacDBforWeb"].ConnectionString);
            bool returnValue = getBool.GetLogin(rfobj.refid,rfobj.password);
            if (returnValue == true)
            {
                string flagvalue = "45c75d19-6b05-43ef-80b3-fe70e2b5aff5";
                return RedirectToAction("UtilityReports", "Admin", new { flagvalue=flagvalue });
            }
            else
            {
                TempData["error"] = "Username or Password incorrect";
                return RedirectToAction("index", "Admin");
            }
        }

       
         
        public ActionResult UploadReport(int flagvalue)
        {
            if (flagvalue == 1)
                return View();
            else
                return RedirectToAction("index", "admin");
        }

        public ActionResult SmsSetup()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UtilityReports(int? page,string flagvalue="2")
        {
            if (flagvalue == "45c75d19-6b05-43ef-80b3-fe70e2b5aff5")
            {
                ILilacDao newobj = new LilacDao(ConfigurationManager.ConnectionStrings["lilacDBforWeb"].ConnectionString);
                IEnumerable<Testmaster> testList = newobj.GetReport();

                int pageSize = 20;
                int pageNumber = (page ?? 1);
                ViewData["total"] = testList.Count();

                List<SelectListItem> report_type = new List<SelectListItem>();
                report_type.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                report_type.Add(new SelectListItem { Text = "DELAYED", Value = "DELAYED" });
                report_type.Add(new SelectListItem { Text = "MODIFIED", Value = "MODIFIED" });

                ViewBag.Status_type = new SelectList(report_type, "Value", "Text");

                return View(testList.ToPagedList(pageNumber, pageSize));
            }
            else
                return RedirectToAction("Index","Admin");
        }

        [HttpPost]
        public ActionResult UtilityReports(FormCollection formCollection,int?page)
        {
            //Fetch Complete report data into testList
            ILilacDao newobj = new LilacDao(ConfigurationManager.ConnectionStrings["lilacDBforWeb"].ConnectionString);
            IEnumerable<Testmaster> testList = newobj.GetReport();
            
            //Initialize FormCollection keys
            string searchString = formCollection["searchString"].ToString();
            string fromDate = formCollection["from"].ToString();
            string toDate = formCollection["todate"].ToString();            
            string status = formCollection["Status_type"].ToString();

            //Check if By Report type is selected
            if (formCollection.AllKeys.Contains("el"))
            {
                List<Testmaster> testList1 = null;
                List<Testmaster> testList2 = new List<Testmaster>();
                string el = formCollection["el"].ToString();
                string[] seperator = { "," };
                string[] words = el.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    string el_type = word;
                    testList1 = testList.Where(s => s.rptname.Contains(el_type)).ToList();
                    testList2.AddRange(testList1);
                }
                testList = testList2;
            }                          
            
            //if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(fromDate) && !String.IsNullOrEmpty(toDate))
            //{
            //    testList = testList.Where(s => s.Refmaster.name.ToUpper().Contains(searchString.ToUpper()));
            //}
            if (!String.IsNullOrEmpty(fromDate) && !String.IsNullOrEmpty(toDate))
            {
                DateTime startdate=Convert.ToDateTime(fromDate);
                DateTime enddate=Convert.ToDateTime(toDate);
                testList = from eff in testList where eff.uploadtime >= startdate && eff.uploadtime<=enddate select eff;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                testList = testList.Where(s => s.Refmaster.name.ToUpper().Contains(searchString.ToUpper()));
            }
            

            if(!String.IsNullOrEmpty(status))
            {
                if (status == "DELAYED")
                {
                    DateTime currentDateTime = DateTime.Now;
                    testList = from eff in testList where eff.rptintime <= currentDateTime && eff.uploadedbit == false select eff;
                }
                if (status == "MODIFIED")
                {
                    testList=testList.Where(s=>s.newtat!=null);
                }
            }

            #region By Status Dropdown items
            List<SelectListItem> report_type = new List<SelectListItem>();
            report_type.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
            report_type.Add(new SelectListItem { Text = "DELAYED", Value = "DELAYED" });
            report_type.Add(new SelectListItem { Text = "MODIFIED", Value = "MODIFIED" });            
            
            ViewBag.Status_type = new SelectList(report_type, "Value", "Text");
            #endregion

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            ViewData["total"] = testList.Count();

            return View(testList.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult ModifyTat(int? page,string flagvalue="2")
        {
            if (flagvalue == "45c75d19-6b05-43ef-80b3-fe70e2b5aff5")
            {
                ILilacDao newobj = new LilacDao(ConfigurationManager.ConnectionStrings["lilacDBforWeb"].ConnectionString);
                IEnumerable<Testmaster> testList = newobj.GetReport();

                int pageSize = 20;
                int pageNumber = (page ?? 1);
                ViewData["total"] = testList.Count();

                return View(testList.ToPagedList(pageNumber, pageSize));
            }
            else
                return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public ActionResult ModifyTat(FormCollection formCollection, int? page)
        {
            ILilacDao newobj = new LilacDao(ConfigurationManager.ConnectionStrings["lilacDBforWeb"].ConnectionString);
            IEnumerable<Testmaster> testList = newobj.GetReport();

            //Initialize FormCollection keys
            string searchString = formCollection["searchString"].ToString();
            string fromDate = formCollection["from"].ToString();
            string toDate = formCollection["to"].ToString();
            

            //Check if By Report type is selected
            if (formCollection.AllKeys.Contains("el"))
            {
                List<Testmaster> testList1 = null;
                List<Testmaster> testList2 = new List<Testmaster>();
                string el = formCollection["el"].ToString();
                string[] seperator = { "," };
                string[] words = el.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    string el_type = word;
                    testList1 = testList.Where(s => s.rptname.Contains(el_type)).ToList();
                    testList2.AddRange(testList1);
                }
                testList = testList2;
            }                     



            if (!String.IsNullOrEmpty(fromDate) && !String.IsNullOrEmpty(toDate))
            {
                DateTime startdate = Convert.ToDateTime(fromDate);
                DateTime enddate = Convert.ToDateTime(toDate);
                testList = from eff in testList where eff.uploadtime >= startdate && eff.uploadtime <= enddate select eff;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                testList = testList.Where(s => s.Refmaster.name.ToUpper().Contains(searchString.ToUpper()));
            }
            
            
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            ViewData["total"] = testList.Count();

            return View(testList.ToPagedList(pageNumber, pageSize));
        }
    }
}
