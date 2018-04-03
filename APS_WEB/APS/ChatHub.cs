using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.Identity;
using APS.Models;
using System.Linq;

namespace APS
{
    [Authorize]
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        private readonly DataAccess objds;

        public ChatHub() {
            objds = new DataAccess();
        }

        public void Send(ChatMessageModel chatMessage)
        {
            chatMessage.UserId = HttpContext.Current.User.Identity.GetUserId();
            chatMessage.UserName = HttpContext.Current.User.Identity.GetUserName();
            // Call the addNewMessageToPage method to update clients.
            chatMessage.Created = DateTime.Now;

            objds.AddHistoryMessages(chatMessage);
            Clients.User(chatMessage.ToUserId).addNewMessageToPage(chatMessage, false);
            Clients.User(chatMessage.UserId).addNewMessageToPage(chatMessage, true);
        }
        public void GetUserList() {
            var currentUser = HttpContext.Current.User.Identity.GetUserId();
            var list = objds.GetUsers().Where(u=> u.ID != currentUser);
            Clients.User(currentUser).getUserList(list);
        }
        public void ChatLoadHistory(string id) {
            var currentUser = HttpContext.Current.User.Identity.GetUserId();
            var history = new ChatMessageHistoryModel
            {
                Id = id,
                Messages = objds.GetHistoryMessages(currentUser, id)
            };
            Clients.User(currentUser).ChatLoadHistory(history);
        }
    }
}