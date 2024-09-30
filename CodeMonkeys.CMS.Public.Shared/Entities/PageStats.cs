using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class PageStats
    {
        [Key]
        public int PageStatsId { get; set; }
        required public string PageUrl { get; set; }
        public int PageVisits { get; set; }
        public int SiteId { get; set; }
        public int PageId { get; set; }
    }
}