using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.ViewModels
{
    public class CustomerPurchaseOrder
    {
        public string CustomerID { get; set; }
        public int OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderDate_str { get; set; }
        public string ContactName { get; set; }
    }
}