using CondemnedAssistance.Helpers;
using CondemnedAssistance.Models;
using CondemnedAssistance.Services.Requirements;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.Resources {
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
            int registerLevelId = Convert.ToInt32(context.User.FindFirst(c => c.Type == "RegisterLevelId").Value);

            List<int> tempRegisters = new List<int> { registerId };
            tempRegisters.AddRange(currentChildren);

            currentChildren = tempRegisters.ToArray();

            if (context.User.IsInRole("3")) {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (resource.ContainsKey("roleId")) {
                int roleId = resource["roleId"];
                if (roleId == 3) {
                    context.Fail();
                    return Task.CompletedTask;
                }
                context.Succeed(requirement);
            }

            if (resource.ContainsKey("levelId")) {
                int requestedRegisterLevel = resource["levelId"];

                int[] registerLevelChildren = registerHelper.GetRegisterLevelChildren(new int[] { }, registerLevelId);

                if(_db.RegisterLevels.Single(r => r.Id == requestedRegisterLevel).IsFirstAncestor) {
                    context.Fail();
                }

                if (registerLevelChildren.Contains(requestedRegisterLevel)) {
                    context.Succeed(requirement);
                }
                else {
                    context.Fail();
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

            if(resource.ContainsKey("parentId") && resource.ContainsKey("levelId")) {
                int parentId = resource["parentId"];
                int requestedRegisterLevel = resource["levelId"];

                int parentRegisterLevelId = _db.Registers.Single(r => r.Id == parentId).RegisterLevelId;

                if(_db.RegisterLevelHierarchies.Any(r => r.ParentLevel == parentRegisterLevelId & r.ChildLevel == requestedRegisterLevel)) {
                    context.Succeed(requirement);
                }
                else {
                    context.Fail();
                }
            }

            if (resource.ContainsKey("childId")) {
                int childId = resource["childId"];
                if (!currentChildren.Any(c => c == childId)) {
                    context.Fail();
                }
                else {
                    context.Succeed(requirement);
                }
            }
            
            return Task.CompletedTask;
        }
    }
}
