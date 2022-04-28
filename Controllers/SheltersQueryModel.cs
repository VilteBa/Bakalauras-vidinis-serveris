using System.ComponentModel;

namespace backend.Controllers
{
    public class SheltersQueryModel
    {
        private const int PageDefault = 0;

        private const int PageLimitDefault = 20;

        [DefaultValue(PageDefault)]
        public int Page { get; set; } = PageDefault;

        [DefaultValue(PageLimitDefault)]
        public int PageLimit { get; set; } = PageLimitDefault;

        public string[] Cities { get; set; }
    }
}
