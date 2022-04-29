using System;
using System.ComponentModel;

namespace backend.Controllers
{
    public class ReservationQueryModel
    {
        private const int PageDefault = 0;

        private const int PageLimitDefault = 20;

        [DefaultValue(PageDefault)]
        public int Page { get; set; } = PageDefault;

        [DefaultValue(PageLimitDefault)]
        public int PageLimit { get; set; } = PageLimitDefault;

        public Guid ShelterId { get; set; }

        public Guid UserId { get; set; }

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }

    }
}
