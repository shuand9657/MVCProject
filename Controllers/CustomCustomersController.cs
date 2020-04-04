using MVCProject.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Controllers
{
    public class CustomCustomersController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: CustomCustomer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(int page = 1 ,int pageSize = 10)
        {
            var customers = db.Customers.AsQueryable().OrderBy(x => x.ContactName).ToPagedList(page, pageSize);
            return View(customers);
        }
    }
}