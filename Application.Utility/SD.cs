using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Utility
{
    public static class SD
    {
        public static class UserRole
        {
            public const string TranineeUser = "Traninee";
            public const string TrainingCenterUser = "Traniner";
            public const string CenterAdminUser = "CenterAdmin";
            public const string ReportUser = "Report";
            public const string AdminUser = "Admin";
            public const string TraingPartnerAdminUser = "TpAdmin";
            public const string TraingPartnerUser = "TpUser";
        }

        public static class CasteCategory
        {
            public const string GeneralCategory = "General";
            public const string ObcCategory = "OBC";
            public const string ScCategory = "SC";
            public const string StCategory = "ST";
        }

        public static class UserCategory
        {
            public const string TrainingCenterUser = "TrainingCenter";
            public const string TrainingPartnerUser = "TrainingPartner";
        }

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

        public static class TrainingCenetrStatus
        {
            public const string ActiveCenter = "Active";
            public const string SuspendedCenter = "Suspended";
            public const string ClosedCenter = "Closed";          
        }

        public const string TrainingCenterId = "SESSION_KEY_TRAINING_CENTER_ID";
        public const string TrainingPartnerId = "SESSION_KEY_TRAINING_PARTNER_ID";
    }
}
