using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    public class PetsQueryModel
    {
        private const int PageDefault = 0;

        private const int PageLimitDefault = 20;

        [DefaultValue(PageDefault)]
        public int Page { get; set; } = PageDefault;

        [DefaultValue(PageLimitDefault)]
        public int PageLimit { get; set; } = PageLimitDefault;

        public int MinAge { get; set; }

        public int MaxAge { get; set; }

        public string[] Sizes { get; set; }

        public string[] Sexes { get; set; }

        public string[] Types { get; set; }

        public string[] Colors { get; set; }

        public string[] Cities { get; set; }
    }
}
