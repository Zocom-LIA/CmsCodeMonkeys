﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public class Site : IEntity
    {
        [Key]
        public int SiteId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public Guid? CreatorId { get; set; }
        public User? Creator { get; set; }

        public int? LandingPageId { get; set; }
        public WebPage? LandingPage { get; set; }
        public ICollection<WebPage> Pages { get; set; } = new List<WebPage>();

        // Explicit Foreign Key
        public int? LandingPageId { get; set; }

        [ForeignKey("LandingPageId")]
        public WebPage? LandingPage { get; set; }

        public object GetIdentifier() => SiteId;
    }
}