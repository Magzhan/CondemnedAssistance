using CondemnedAssistance.Helpers;
using CondemnedAssistance.Models;
using CondemnedAssistance.Services.Requirements;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.Resources
{
    public class ResourceRegisterHandler : AuthorizationHandler<ResourceRegisterBasedRequirement, Dictionary<string, int>> {

        private UserContext _db;
        private RegisterHelper registerHelper;

        public ResourceRegisterHandler(UserContext context) {
            _db = context;
            registerHelper = new RegisterHelper(context);
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceRegisterBasedRequirement requirement, Dictionary<string, int> resource) {

            int[] currentChildren = registerHelper.GetRegisterChildren(new int[] { }, Convert.ToInt32(context.User.FindFirst(c => c.Type == "RegisterId").Value));

            int registerId = Convert.ToInt32(context.User.FindFirst(c => c.Type == "RegisterId").Value);
            
            if (context.User.IsInRole("3")) {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (resource.ContainsKey("userId")) {
                int userId = resource["userId"];
                int userRoleId = _db.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;
                if (userRoleId == 3) {
                    context.Fail();
                    return Task.CompletedTask;
                }
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

                if (currentChildren.Any(c => c == parentId) || (registerId == parentId)) {
                    context.Succeed(requirement);
                } else {
                    context.Fail();
                }
            }

            if (resource.ContainsKey("childId")) {
                int childId = resource["childId"];
                if (!currentChildren.Any(c => c == childId)) {
                    context.Fail();
                }
            }
            
            return Task.CompletedTask;
        }
    }
}
