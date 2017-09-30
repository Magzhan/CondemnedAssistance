using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.Security.Register {
    public static class RegisterOperations {

        public static readonly OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = _Constants.Constants.Create };
        public static readonly OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = _Constants.Constants.Read };
        public static readonly OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement { Name = _Constants.Constants.Update };
        public static readonly OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = _Constants.Constants.Delete };

    }
}
