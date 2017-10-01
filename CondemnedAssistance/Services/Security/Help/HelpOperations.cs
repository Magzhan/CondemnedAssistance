using CondemnedAssistance.Services.Security._Constants;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace CondemnedAssistance.Services.Security.Help {
    public static class HelpOperations {
        public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = Constants.Create };
        public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = Constants.Read };
        public static OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement { Name = Constants.Update };
        public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = Constants.Delete };
        public static OperationAuthorizationRequirement UserHelpList = new OperationAuthorizationRequirement { Name = Constants.UserHelpList }; 
        public static OperationAuthorizationRequirement AddUserHelp = new OperationAuthorizationRequirement { Name = Constants.AddUserHelp };
    }
}
