using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Td2013.Demos.SignalR.Models
{
    public class HtmlHeroUnit
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public string MoreButton { get; set; }
    }

    public class HtmlNavLink
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class HtmlMainTitle
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
    }

    public class HtmlTopNav
    {
        public HtmlMainTitle TopNav { get; set; }
        public IEnumerable<HtmlNavLink> Links { get; set; }
    }
}