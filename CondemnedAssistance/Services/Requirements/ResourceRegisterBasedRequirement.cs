using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.Requirements {
    public class ResourceRegisterBasedRequirement : IAuthorizationRequirement{
    }

    public class ResourceRegisterDataClass {
        public Dictionary<string, int> Keys { get; set; }

        public int[] RegisterChildren { get; set; }

        public ResourceRegisterDataClass(Dictionary<string, int> keys, int[] children) {
            Keys = keys;
            RegisterChildren = children;
        }
    }
}
