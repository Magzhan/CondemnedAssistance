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
        private ApplicationContext _app;

        public RoleAuthorizationHandler(UserContext context, ApplicationContext app) {
            _db = context;
            _app = app;
        }


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, int resource) {
            int roleId = _db.UserRoles.Single(r => r.UserId == Convert.ToInt32(context.User.Identity.Name)).RoleId;
            int actionId = _app.Actions.Single(a => a.ControllerId == resource & a.NormalizedName == requirement.Name).Id;

            if (_app.RoleAccesses.Any(r => r.ControllerId == resource & r.ActionId == actionId & r.RoleId == roleId & r.IsAllowed)) {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
