﻿using CondemnedAssistance.Services.Security._Constants;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.Security.Register {
    public static class RegisterOperations {

        public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = Constants.Create };
        public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = Constants.Read };
        public static OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement { Name = Constants.Update };
        public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = Constants.Delete };
        public static OperationAuthorizationRequirement RegisterLevels = new OperationAuthorizationRequirement { Name = Constants.RegisterLevels };
        public static OperationAuthorizationRequirement CreateLevel = new OperationAuthorizationRequirement { Name = Constants.CreateLevel };
        public static OperationAuthorizationRequirement UpdateLevel = new OperationAuthorizationRequirement { Name = Constants.UpdateLevel };
        public static OperationAuthorizationRequirement DeleteLevel = new OperationAuthorizationRequirement { Name = Constants.DeleteLevel };

    }
}
