using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintyNineKartStore.Utility
{
    public static class SD
    {
        public const string Role_User_Individual = "Individual";
        public const string Role_User_Company = "Company";
        public const string Role_User_Admin = "Admin";
        public const string Role_User_Employee = "Employee";

        public static class OrderStatus
        {
            public const string OrderPending = "Pending";
            public const string OrderApproved = "Approved";
            public const string OrderInProcess = "InProcess";
            public const string OrderShipped = "Shipped";
            public const string OrderCancelled = "Cancelled";
            public const string OrderDelivered = "Delivered";
        }

        public static class PaymentStatus
        {
            public const string PaymentPending = "Pending";
            public const string PaymentApproved = "Approved";
            public const string PaymentDelayed = "ApprovedForDelayedPayment";
            public const string Rejected = "Rejected";
            public const string Refunded = "Refunded";
        }
        public class ShoppinghCart
        {
            public const string ShoppingCartItemsCount = "SessionShoppingCartCount";
        }
    }
}
