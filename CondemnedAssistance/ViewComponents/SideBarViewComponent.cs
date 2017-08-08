using CondemnedAssistance.Helpers;
using CondemnedAssistance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CondemnedAssistance.ViewComponents {
    public class SideBarViewComponent : ViewComponent{

        private readonly UserContext _db;

        public SideBarViewComponent(UserContext context) {
            _db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(LinkClass[] links) {
            var thisUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(HttpContext.User.Identity.Name));
            List<LinkFormatted> formattedLinks = new List<LinkFormatted>();

            foreach(var l in links) {
                string fullLink = "/" + l.Controller + "/" + l.Action;
                if(l.RouteValues.Count > 0) {
                    fullLink += "?";
                    foreach (KeyValuePair<string, string> routeValue in l.RouteValues) {
                        fullLink += routeValue.Key + "=" + routeValue.Value + "&";
                    }
                    fullLink.Remove(fullLink.Length-1);
                }
                formattedLinks.Add(new LinkFormatted { FullLink = fullLink, LinkText = l.Text, LinkClass = ((l.IsSelected) ? "selected-sidebar-menu" : "") });
            }

            return View(formattedLinks);
        }

    }

    public class LinkFormatted {
        public string FullLink { get; set; }
        public string LinkText { get; set; }
        public string LinkClass { get; set; }
    }
}
