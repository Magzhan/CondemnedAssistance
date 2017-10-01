using CondemnedAssistance.Services.Security._Constants;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.Security.Message
{
    public static class MessageOperations {
        public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = Constants.Read };
        public static OperationAuthorizationRequirement LoadUsers = new OperationAuthorizationRequirement { Name = Constants.LoadUsers };
        public static OperationAuthorizationRequirement LoadMessages = new OperationAuthorizationRequirement { Name = Constants.LoadMessages };
        public static OperationAuthorizationRequirement Send = new OperationAuthorizationRequirement { Name = Constants.Send };
    }
}
