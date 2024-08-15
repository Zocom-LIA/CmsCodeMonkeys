using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class PageStats
    {
        public int Id { get; set; }
        required public string PageUrl { get; set; }
        public int PageVisits { get; set; }
    }
}