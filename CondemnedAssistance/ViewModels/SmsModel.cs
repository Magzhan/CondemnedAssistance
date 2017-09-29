using CondemnedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.ViewModels {
    public class SmsModel : Sms{
        public int UserId { get; set; }
    }
}
