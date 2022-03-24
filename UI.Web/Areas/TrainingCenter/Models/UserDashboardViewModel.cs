using NsdcTraingPartnerHub.Service.Models;

namespace NsdcTraingPartnerHub.Web.Areas.TrainingCenter.Models
{
    public class UserDashboardViewModel
    {
        public string Name { get; set; }
        public string UserCategory { get; set; }
        public  TrainingCenterDto TrainingCenter { get; set; }
        public TrainingPartnerDto TrainingPartner{ get; set; }
        public string UserRole { get; set; }

    }
}
