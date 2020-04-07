using MVCProject.Models;
using MVCProject.ViewModels;
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


        //這邊要透過LinQ做table 串聯
        public ActionResult PurchaseOrder(int page = 1, int pageSize = 10)
        {
            //養成習慣，用try catch把程式碼包起來，這樣比較好除錯
            //List<CustomerPurchaseOrder> customerList = new List<CustomerPurchaseOrder>();
            try
            {
                var query = (from a in db.Customers
                                join b in db.Orders on a.CustomerID equals b.CustomerID
                                orderby b.OrderDate descending
                                select new CustomerPurchaseOrder()
                                {
                                    CustomerID = a.CustomerID,
                                    OrderID = b.OrderID,
                                    OrderDate = b.OrderDate,
                                    ContactName = a.ContactName,
                                    OrderDate_str = b.OrderDate.HasValue ? b.OrderDate.Value.ToString() : ""
                                }
                                ).ToPagedList(page,pageSize);
            

                return View(query);
            }
            catch (Exception ex)
            {
                //紀錄錯誤訊息

                throw ex;
            }
        }

        /// <summary>
        /// 根據訂單編號將訂單資訊抓出來OrderID
        /// </summary>
        /// <param name="id">訂單編號</param>
        /// <returns></returns>
        public ActionResult OrderDetail(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(id.ToString())) //如果沒有ID(訂單編號),就
                    return RedirectToAction("PurchaseOrder");

                OrderViewModel orderItem = new OrderViewModel();

                using (db)
                {
                    orderItem = (from a in db.Orders
                                 join b in db.Customers on a.CustomerID equals b.CustomerID
                                 join c in db.Employees on a.EmployeeID equals c.EmployeeID
                                 join d in db.Order_Details on a.OrderID equals d.OrderID
                                 join e in db.Shippers on a.ShipVia equals e.ShipperID
                                 where a.OrderID.Equals(id)
                                 select new OrderViewModel()
                                 {
                                     OrderID = a.OrderID,
                                     CustomerID = a.CustomerID,
                                     EmployeeID = a.EmployeeID,
                                     Freight = a.Freight,
                                     ShipName = a.ShipName,
                                     ShipAddress = a.ShipAddress,
                                     ShipCity = a.ShipCity,
                                     ShipRegion = a.ShipRegion,
                                     ShipPostalCode = a.ShipPostalCode,
                                     ShipCountry = a.ShipCountry,
                                     Str_OrderDate = a.OrderDate.HasValue? a.OrderDate.Value.ToString() : string.Empty ,
                                     Str_RequiredDate = a.RequiredDate.HasValue ? a.RequiredDate.Value.ToString() : string.Empty,
                                     Str_ShippedDate = a.ShippedDate.HasValue? a.ShippedDate.Value.ToString(): string.Empty,
                                     CustomerName = b.ContactName,
                                     EmployeeName = c.FirstName + " " + c.LastName,
                                     ShipVia = a.ShipVia,
                                     ShipperName = e.CompanyName
                                 }).FirstOrDefault();
                }
                if (orderItem != null)
                    return View(orderItem);
                else
                    return RedirectToAction("PurchaseOrder");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}