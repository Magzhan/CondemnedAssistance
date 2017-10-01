using CondemnedAssistance.Services.Security._Constants;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace CondemnedAssistance.Services.Security.Event {
    public static class EventOperations {

        public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = Constants.Create };
        public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = Constants.Read };
        public static OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement { Name = Constants.Update };
        public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = Constants.Delete };

        public static OperationAuthorizationRequirement EventStatuses = new OperationAuthorizationRequirement { Name = Constants.EventStatuses };
        public static OperationAuthorizationRequirement CreateStatus = new OperationAuthorizationRequirement { Name = Constants.CreateStatus };
        public static OperationAuthorizationRequirement UpdateStatus = new OperationAuthorizationRequirement { Name = Constants.UpdateStatus };
        public static OperationAuthorizationRequirement DeleteStatus = new OperationAuthorizationRequirement { Name = Constants.DeleteStatus };
    }
}
