using CondemnedAssistance.Helpers;
using CondemnedAssistance.Hubs;
using CondemnedAssistance.Models;
using CondemnedAssistance.Services.Security._Constants;
using CondemnedAssistance.Services.Security.Message;
using CondemnedAssistance.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace CondemnedAssistance.Controllers {
    public class MessageController : Microsoft.AspNetCore.Mvc.Controller {

        private UserContext _db;
        private ApplicationContext _app;
        private RegisterHelper registerHelper;
        private WebSocketMessageHandler _webScoketMessageHandler;
        private IAuthorizationService _authorizationService;
        private int _controllerId;

        public MessageController(UserContext context, ApplicationContext app, WebSocketMessageHandler webSocketMessageHandler, IAuthorizationService authorizationService) {
            _db = context;
            _app = app;
            registerHelper = new RegisterHelper(context);
            _webScoketMessageHandler = webSocketMessageHandler;
            _authorizationService = authorizationService;
            _controllerId = _app.Controllers.Single(c => c.NormalizedName == Constants.Message).Id;
        }

        [HttpGet]
        public async Task<IActionResult> Index(){
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, MessageOperations.Read);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }
            return View("Messages");
        }

        [HttpGet]
        public async Task<IActionResult> LoadUsers(int helpId = 0) {

            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, MessageOperations.LoadUsers);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            int[] currUserRegisterIds = registerHelper.GetRegisterChildren(new int[] { }, Convert.ToInt32(User.FindFirst(c => c.Type == "RegisterId").Value));
            List<int> tempRegisters = new List<int>() { Convert.ToInt32(User.FindFirst(c => c.Type == "RegisterId").Value) };
            tempRegisters.AddRange(currUserRegisterIds);
            currUserRegisterIds = tempRegisters.ToArray();
            int[] userIdsByRegister = _db.UserRegisters.Where(r => currUserRegisterIds.Contains(r.RegisterId)).Select(r => r.UserId).ToArray();
            int[] allowedUserIds;
            if (User.IsInRole("2")) {
                int[] userIdsByRole = _db.UserRoles.Where(r => r.RoleId == 1).Select(r => r.UserId).ToArray();
                allowedUserIds = userIdsByRole.Intersect(userIdsByRegister).ToArray();
            }else if (User.IsInRole("1")) {
                int[] userIdsByRole = _db.UserRoles.Where(r => r.RoleId == 2).Select(r => r.UserId).ToArray();
                int[] userIdsByHelp = _app.UserHelps.Where(h => h.HelpId == helpId).Select(h => h.UserId).ToArray();
                currUserRegisterIds = registerHelper.GetRegisterParents(new int[] { }, Convert.ToInt32(User.FindFirst(c => c.Type == "RegisterId").Value));
                userIdsByRegister = _db.UserRegisters.Where(r => currUserRegisterIds.Contains(r.RegisterId)).Select(r => r.UserId).ToArray();
                allowedUserIds = userIdsByRegister.Intersect(userIdsByRole).ToArray();
                allowedUserIds = allowedUserIds.Intersect(userIdsByHelp).ToArray();
            }
            else {
                int[] userIdsByRole = _db.UserRoles.Select(r => r.UserId).ToArray();
                allowedUserIds = userIdsByRegister.Intersect(userIdsByRole).ToArray();
            }

            List<UserModelCreate> model = new List<UserModelCreate>();

            _db.UserInfo.Where(u => allowedUserIds.Contains(u.UserId)).ToList().ForEach(row => {

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
        public async Task<IActionResult> LoadMessages(int receiverId) {

            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, MessageOperations.LoadMessages);

            if (!result.Succeeded) {
                return new ChallengeResult();
            }

            int currUserId = Convert.ToInt32(HttpContext.User.Identity.Name);

            List<MessageExchange> dialogs = _app.MessageExchanges.Where(m => m.SenderId == currUserId & m.ReceiverId == receiverId).ToList();
            List<Message> messages = new List<Message>();

            dialogs.ForEach(dialog => {
                messages.Add(_app.Messages.Single(m => m.Id == dialog.MessageId));
            });

            return PartialView(messages);
        }

        [HttpGet]
        public async Task Send(int receiverId, int registerId, int roleId, [FromQueryAttribute] string message, int helpId = 1) {

            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, _controllerId, MessageOperations.Send);

            if (!result.Succeeded) {
                new ChallengeResult();
            }

            WebSocket socket = _webScoketMessageHandler.GetCurrentUserSocket(receiverId);

            if(socket != null) {
                await _webScoketMessageHandler.SendMessageAsync(receiverId, registerId, roleId, message);
            }

            Message thisMessage = new Message {
                    IsRead = false,
                    IsReceived = true,
                    IsSent = true,
                    SentDate = DateTime.Now,
                    ReceivedDate = DateTime.Now,
                    SenderId = Convert.ToInt32(User.Identity.Name),
                    Text = message
                };
                _app.Messages.Add(thisMessage);

                await _app.SaveChangesAsync();

                _app.MessageExchanges.Add(new MessageExchange {
                    HelpId = helpId,
                    MessageId = thisMessage.Id,
                    ReceiverId = receiverId,
                    SenderId = Convert.ToInt32(User.Identity.Name)
                });

                await _app.SaveChangesAsync();
        }
    }
}
