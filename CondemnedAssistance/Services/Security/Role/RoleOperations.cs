using CondemnedAssistance.Services.Security._Constants;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace CondemnedAssistance.Services.Security.Role {
    public static class RoleOperations {
        public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = Constants.Create };
        public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = Constants.Read };
        public static OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement { Name = Constants.Update };
        public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = Constants.Delete };
        public static OperationAuthorizationRequirement RoleAccess = new OperationAuthorizationRequirement { Name = Constants.RoleAccess };
    }
}
