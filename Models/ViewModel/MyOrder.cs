using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackEndMyStore_25dh110949.Models.ViewModel;
using BackEndMyStore_25dh110949.Models;

namespace BackEndMyStore_25dh110949.Models.ViewModel
{
    public class MyOrder
    {
        public string SearchTerm { get; set; } 
        public List<Order> AllOrders { get; set; } = new List<Order>();

        public List<Order> PendingPaymentOrders { get; set; } = new List<Order>();
        public List<Order> ProcessingOrders { get; set; } = new List<Order>();
        public List<Order> ShippingOrders { get; set; } = new List<Order>();
        public List<Order> DeliveredOrders { get; set; } = new List<Order>();
        public List<Order> CancelledOrders { get; set; } = new List<Order>();
    }
}