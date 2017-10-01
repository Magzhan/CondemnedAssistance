using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.Security._Constants {
    public class Constants {
        //Controllers
        public static readonly string Address = "Address";
        public static readonly string Education = "Education";
        public static readonly string Email = "Email";
        public static readonly string Event = "Event";
        public static readonly string Help = "Help";
        public static readonly string Message = "Message";
        public static readonly string Profession = "Profession";
        public static readonly string Register = "Register";
        public static readonly string Role = "Role";
        public static readonly string Sms = "Sms";
        public static readonly string User = "User";
        public static readonly string Vacancy = "Vacancy";

        //Basic actions
        public static readonly string Create = "CREATE";
        public static readonly string Read = "READ";
        public static readonly string Update = "UPDATE";
        public static readonly string Delete = "DELETE";

        //Address Controller
        public static readonly string GetAddressList = "GET_ADDRESS_LIST";
        public static readonly string AddressLevels = "ADDRESS_LEVELS";
        public static readonly string CreateLevel = "CREATE_LEVEL";
        public static readonly string UpdateLevel = "UPDATE_LEVEL";
        public static readonly string DeleteLevel = "DELETE_LEVEL";

        //Event Controller
        public static readonly string EventStatuses = "EVENT_STATUSES";
        public static readonly string CreateStatus = "CREATE_STATUS";
        public static readonly string UpdateStatus = "UPDATE_STATUS";
        public static readonly string DeleteStatus = "DELETE_STATUS";

        //Help Controller
        public static readonly string UserHelpList = "USER_HELP_LIST";
        public static readonly string AddUserHelp = "ADD_USER_HELP";

        //Message Controller
        public static readonly string LoadUsers = "LOAD_USERS";
        public static readonly string LoadMessages = "LOAD_MESSAGES";
        public static readonly string Send = "SEND";

        //Register Controller
        public static readonly string RegisterLevels = "REGISTER_LEVELS";

        //User Controller
        public static readonly string History = "HISTORY";
        public static readonly string HistoryDetail = "HISTORY_DETAIL";
    }
}
