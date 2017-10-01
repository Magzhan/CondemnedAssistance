using CondemnedAssistance.Services.Security._Constants;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace CondemnedAssistance.Services.Security.User {
    public static class UserOperations {
        public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = Constants.Create };
        public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = Constants.Read };
        public static OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement { Name = Constants.Update };
        public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = Constants.Delete };
        public static OperationAuthorizationRequirement History = new OperationAuthorizationRequirement { Name = Constants.History };
        public static OperationAuthorizationRequirement HistoryDetail = new OperationAuthorizationRequirement { Name = Constants.HistoryDetail };
    }
}
