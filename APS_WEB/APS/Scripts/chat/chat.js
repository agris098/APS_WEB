$(function () {
    function ChatManager() {
        var chatHub,
            chatContainer = ".chat-container",
            chatWindow = ".chat-window",
            messageContainer = ".chat-messages",
            messageInput = "#message";
            chatManager = "#chat-manager",
            chatManagerBody = ".chat-manager-body";

        function init() {
            $(chatContainer).show();
            initChatHub();
            registerHubEvents();
            registerChatEvents();
            registerOtherEvents();
        }

        function initChatHub() {
            chatHub = $.connection.chatHub;
        }

        function registerHubEvents() {
            chatHub.client.addNewMessageToPage = function (message,own) {
                // Add the message to the page. 
                var side = !own ? "left" : "right",
                    userId = own ? message.ToUserId : message.UserId,
                    userName = own ? message.ToUserName : message.UserName,
                    mBody = chatById(userId).find(messageContainer);

                if (chatIsOpen(userId)) {
                    mBody.append(createMessage(message, side));
                } else {
                    addNewChat(userId, userName);
                    mBody.append(createMessage(message, side));
                }

            };
            chatHub.client.GetUserList = function (list) {
                var container = $(chatContainer + " " + chatManagerBody);
                container.empty();
                $.each(list, function () {
                    container.append("<div user-id=" + this.ID + " user-name=" + this.UserName +" class='user'>" + this.UserName + "</div>");
                });
            };
            chatHub.client.ChatLoadHistory = function (history) {
                var container = chatById(history.Id),
                    mContainer = container.find(messageContainer)

                mContainer.empty();
                $.each(history.Messages, function () {
                    var side = this.UserId !== container.attr("user-id") ? "right" : "left";
                    mContainer.append(createMessage(this, side));
                });
            } 
        }

        function registerChatEvents() {
            $(chatContainer).on("click",".chat-header", function () {
                $(this).parent().toggleClass("show-body");
            });
            $(chatContainer).on("click", "[chat-window-close]", function (e) {
                e.stopPropagation();
                $(this).closest(chatWindow).remove();
            });
            $(chatContainer).on("click", ".chat-manager-add", function () {
                chatHub.server.getUserList();
                $(this).parent().addClass("show-manager");
            });
            $(chatContainer).on("click", chatManagerBody + " .user" , function () {
                var userId = $(this).attr("user-id"),
                    userName = $(this).attr("user-name");

                if (chatIsOpen(userId)) {
                    setChatWindowActive(userId);
                } else {
                    addNewChat(userId, userName );
                }
            });
            $(chatContainer).on("click", ".chat-manager-header", function () {
                $(this).parent().parent().removeClass("show-manager");
            });
            $(chatContainer).on("click", "#sendmessage", function () {
                var window = $(this).closest(chatWindow),
                    userid = window.attr("user-id"),
                    username = window.attr("user-name"),
                    message = window.find(messageInput);

                if (message.val().trim() === '') {
                    return false;
                }
                chatHub.server.send(
                    {
                        ToUserId: userid,
                        ToUserName: username,
                        Message : message.val()
                    }
                );
                message.val('');
                message.focus();
            });
            $(chatContainer).on("keydown", messageInput, function (e) {
                if (e.which === 13) {
                    $(this).parent().find("#sendmessage").trigger("click");
                }
            });
        }
        function registerOtherEvents() {
            $(document).on("click", ".profile .sendMessage", function () {
                var profile = $('.profile'),
                    userId = profile.data("userid"),
                    userName = profile.find('.FullName').html();

                if (chatIsOpen(userId)) {
                    setChatWindowActive(userId);
                } else {
                    addNewChat(userId, userName);
                }
            });
        }
        function chatById(id) {
            return $(chatWindow+"[user-id="+id+"]");
        }
        function chatIsOpen(id) {
            if (chatById(id).length === 0) {
                return false;
            } else {
                return true
            }     
        }
        function addNewChat(id, userName) {
            var newChat = $(chatContainer + " #template").clone();
            newChat.attr("id", "chat").attr("user-id", id).attr("user-name", userName);
            newChat.find(".chat-header span").html(encodeText(userName));
            newChat.addClass("show-body");
            $(chatContainer).append(newChat);
            chatHub.server.chatLoadHistory(id);
        }
        function createMessage(message,side) {
            return $('<div data-toggle="tooltip" data-placement="' + side + '" title="' + message.DateTime + '" class="message ' + side + '">'// + encodeText(id)
                + encodeText(message.Message) + '</div>').tooltip();
        }
        function setChatWindowActive(id, status) {
            var window = chatById(id);
            if (status === undefined || status === true) {
                window.addClass("show-body");
            } else {
                window.removeClass("show-body");
            }          
        }
        function encodeText(text) {
            return $('<div />').text(text).html();
        }
        function start() {
            $.connection.hub.start();
        }
        function stop() {
            $.connection.hub.stop();
        }      
        return {
            init: init,
            start: start,
            stop: stop
        };
    }
    $(document).ready(function () {
        var chatManager = new ChatManager();
        if (currentUser.Id)
        {
            chatManager.init();
            chatManager.start();
        }

    });
});