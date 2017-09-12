using CondemnedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Helpers {
    public class LinkHelper {

        private readonly UserContext _db;

        private Dictionary<string, List<LinkClass>> collection;

        public LinkHelper(UserContext context, string currentCollection) {
            _db = context;
            collection = new Dictionary<string, List<LinkClass>>();
            collection.Add("userProfile", new List<LinkClass> {
                new LinkClass { Controller = "Account", Action = "Profile", IsSelected = false, Text = "Профиль"},
                new LinkClass { Controller = "Account", Action = "ChangePassword", IsSelected = false, Text = "Изменить пароль"},
                new LinkClass { Controller = "", Action = "", Text = "Входящие", IsSelected = false}
            });
            collection.Add("userEdit", new List<LinkClass> {
                new LinkClass { Controller = "User", Action = "Update", IsSelected = false, Text = "Персональные данные" },
                new LinkClass { Controller = "", Action = "", IsSelected = false, Text = "Пробация" },
                new LinkClass { Controller = "", Action = "", IsSelected = false, Text = "История" }
            });
        }

        public LinkClass[] GetLinks(string currentController, string currentAction) {
            foreach(KeyValuePair<string, List<LinkClass>> link in collection) {
                foreach(LinkClass linkClass in link.Value) {
                    if(linkClass.Controller == currentController && linkClass.Action == currentAction) {
                        linkClass.IsSelected = true;
                    }
                    else {
                        linkClass.IsSelected = false;
                    }
                }
            }
            return collection.First().Value.ToArray();
        }
    }

    public class LinkClass {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Text { get; set; }
        public bool IsSelected { get; set; }
        public Dictionary<string, string> RouteValues { get; set; }
        public LinkClass() {
            RouteValues = new Dictionary<string, string>();
        }
    }
}
