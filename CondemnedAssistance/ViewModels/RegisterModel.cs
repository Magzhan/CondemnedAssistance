﻿using CondemnedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.ViewModels {
    public class RegisterModel {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int RegisterLevelId { get; set; }

        public RegisterLevel RegisterLevel { get; set; }
    }

    public class RegisterLevelModel {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
