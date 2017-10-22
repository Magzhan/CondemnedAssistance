using CondemnedAssistance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.Security.RoleAuthorization {
    public class RoleAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, int> {

        private UserContext _db;

        public RoleAuthorizationHandler(UserContext context) {
            _db = context;
        }


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, int resource) {
            int roleId = _db.UserRoles.Single(r => r.UserId == Convert.ToInt32(context.User.Identity.Name)).RoleId;
            int actionId = _db.Actions.Single(a => a.ControllerId == resource & a.NormalizedName == requirement.Name).Id;

            if (_db.RoleAccesses.Any(r => r.ControllerId == resource & r.ActionId == actionId & r.RoleId == roleId & r.IsAllowed)) {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
