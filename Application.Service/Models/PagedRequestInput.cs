namespace NsdcTraingPartnerHub.Service.Models
{
    public class PagedRequestInput
    {
        const int maxPageSize = 100;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = (pageSize > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
