using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using CBApi;
using CBApi.Models;
using CBApi.Models.Responses;
using CBApi.Models.Service;
using FastClickJobs.Models;


namespace FastClickJobs.Controllers
{
    public class HomeController : Controller
    {
        public readonly string careerbuilder = "http://api.careerbuilder.com/v1/jobsearch?DeveloperKey=WDHR4SN696HYPP3GYBH3";

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            RedirectToAction("Test");
            return View();
        }

        public ActionResult Test()
        {
            XmlDocument test = new XmlDocument();
            
            string cb = "http://api.careerbuilder.com/v1/jobsearch?DeveloperKey=WDHR4SN696HYPP3GYBH3&Skills=Programming";
            //XElement linq = new XElement(;
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(cb);
            request.Method = "GET";
            request.ContentType = "application/xml";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string outString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //test.LoadXml(outString);
            XDocument.Load(outString);
            //RedirectToAction("An");
            return View(test);
        }

        [HttpGet]
        public ActionResult An()
        { 
            return View();
        }


        [HttpPost]
        public ActionResult An(JobRequest request)
        {
            var svc = API.GetInstance("WDHR4SN696HYPP3GYBH3");
            //List<Category> codes = svc.GetCategories().WhereCountryCode(CBApi.Models.Service.CountryCode.US).ListAll();

            //Make a call to https://api.careerbuilder.com/v1/employeetypes
            List<EmployeeType> emps = svc.GetEmployeeTypes()
                                         .WhereCountryCode(CountryCode.US)
                                         .ListAll();
            //Search for Jobs
            var search = svc.JobSearch()
                            .WhereKeywords(request.Keywords)
                            .WhereLocation(request.Location)
                            .WhereCountryCode(CountryCode.US)
                            .OrderBy(OrderByType.Title)
                            .Ascending()
                            .Search();
            var jobs = search.Results;
            foreach (JobSearchResult item in jobs)
            {
                Console.WriteLine(item.JobTitle);
            }

            Job myJob = svc.GetJob(jobs.First().DID);

            return PartialView("Testsearchresults", jobs);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Job(string id)
        {
            var svc = API.GetInstance("WDHR4SN696HYPP3GYBH3");
            Job myJob = svc.GetJob(id);
            return View(myJob);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
