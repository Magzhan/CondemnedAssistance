using CondemnedAssistance.Helpers;
using CondemnedAssistance.Hubs;
using CondemnedAssistance.Models;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class MessageController : Controller {

        private UserContext _db;
        private RegisterHelper registerHelper;
        private WebSocketMessageHandler _webScoketMessageHandler;

        public MessageController(UserContext context, WebSocketMessageHandler webSocketMessageHandler) {
            _db = context;
            registerHelper = new RegisterHelper(context);
            _webScoketMessageHandler = webSocketMessageHandler;
        }

        [HttpGet]
        public IActionResult Index(){

            return View("Messages");
        }

        [HttpGet]
        public IActionResult LoadUsers(int helpId = 0) {
            int[] currUserRegisterIds = registerHelper.GetRegisterChildren(new int[] { }, Convert.ToInt32(User.FindFirst(c => c.Type == "RegisterId").Value));
            int[] userIdsByRegister = _db.UserRegisters.Where(r => currUserRegisterIds.Contains(r.RegisterId)).Select(r => r.UserId).ToArray();
            int[] allowedUserIds;
            if (User.IsInRole("2")) {
                int[] userIdsByRole = _db.UserRoles.Where(r => r.RoleId == 1).Select(r => r.UserId).ToArray();
                allowedUserIds = userIdsByRole.Intersect(userIdsByRegister).ToArray();
            }else if (User.IsInRole("1")) {
                int[] userIdsByRole = _db.UserRoles.Where(r => r.RoleId == 2).Select(r => r.UserId).ToArray();
                int[] userIdsByHelp = _db.UserHelps.Where(h => h.HelpId == helpId).Select(h => h.UserId).ToArray();
                currUserRegisterIds = registerHelper.GetRegisterParents(new int[] { }, Convert.ToInt32(User.FindFirst(c => c.Type == "RegisterId").Value));
                userIdsByRegister = _db.UserRegisters.Where(r => currUserRegisterIds.Contains(r.RegisterId)).Select(r => r.UserId).ToArray();
                allowedUserIds = userIdsByRegister.Intersect(userIdsByRole).ToArray();
                allowedUserIds = allowedUserIds.Intersect(userIdsByHelp).ToArray();
            }
            else {
                int[] userIdsByRole = _db.UserRoles.Select(r => r.UserId).ToArray();
                allowedUserIds = userIdsByRegister.Intersect(userIdsByRole).ToArray();
            }

            List<int> tempList = allowedUserIds.ToList();
            tempList.Add(Convert.ToInt32(User.FindFirst(c => c.Type == "RegisterId").Value));
            allowedUserIds = tempList.ToArray();

            List<UserModelCreate> model = new List<UserModelCreate>();

            _db.UserStaticInfo.Where(u => allowedUserIds.Contains(u.UserId)).ToList().ForEach(row => {

                int roleId = _db.UserRoles.Single(r => r.UserId == row.UserId).RoleId;
                int registerId = _db.UserRegisters.Single(r => r.UserId == row.UserId).RegisterId;

                model.Add(new UserModelCreate {
                    UserId = row.UserId,
                    LastName = row.LastName,
                    FirstName = row.FirstName,
                    MiddleName = row.MiddleName,
                    RoleId = roleId,
                    UserRegisterId = registerId
                });
            });

            return PartialView(model);
        }

        [HttpGet]
        public async Task Send(int receiverId, int registerId, int roleId, [FromQueryAttribute] string message) {
            await _webScoketMessageHandler.SendMessageAsync(receiverId, registerId, roleId, message);
        }
    }
}
