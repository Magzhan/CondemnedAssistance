using CondemnedAssistance.Services.Requirements;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.Resources {
    public class ResourceRegisterHandler : AuthorizationHandler<ResourceRegisterBasedRequirement, Dictionary<string, int>> {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceRegisterBasedRequirement requirement, Dictionary<string, int> resource) {
            
            if (context.User.IsInRole("3")) {
                context.Succeed(requirement);
            }

            if (resource.ContainsKey("levelId")) {
                int requestedRegisterLevel = resource["levelId"];

                switch (Convert.ToInt32(context.User.FindFirst(c => c.Type == "RegisterLevelId").Value)) {
                    case 1:
                        if (requestedRegisterLevel == 1) {
                            context.Fail();
                        } 
                        break;
                    case 2:
                        if (requestedRegisterLevel < 3) {
                            context.Fail();
                        }
                        break;
                    case 3:
                    default:
                        context.Fail();
                        break;
                }
            }

            if (resource.ContainsKey("parentId")) {
                int parentId = resource["parentId"];

                if (Convert.ToInt32(context.User.FindFirst(c => c.Type == "RegisterId").Value) != parentId) {
                    context.Fail();
                }
            }

            if (resource.ContainsKey("childId")) {
                int childId = resource["childId"];
                if (!context.User.FindAll(c => c.Type == "RegisterChildId").Any(c => Convert.ToInt32(c.Value) == childId)) {
                    context.Fail();
                }
            }
            
            return Task.CompletedTask;
        }
    }
}
