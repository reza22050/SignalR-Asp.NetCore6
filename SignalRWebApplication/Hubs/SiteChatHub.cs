using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRWebApplication.Models.Services;

namespace SignalRWebApplication.Hubs
{
    public class SiteChatHub:Hub
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IMessageService _messageService;
        public SiteChatHub(IChatRoomService chatRoomService, IMessageService messageService)
        {
            _chatRoomService = chatRoomService;
            _messageService = messageService;
        }
        public async Task SendNewMessage(string sender, string message)
        {
            //Send message to all clients
            //await Clients.All.SendAsync("GetNewMessage",sender,message, DateTime.Now);

            var roomId = await _chatRoomService.GetChatRoomForConnection(Context.ConnectionId);

            MessageDto messageDto = new MessageDto()
            {
                Sender = sender,
                Message = message,
                Time = DateTime.Now
            };
            await _messageService.SaveChatMessage(roomId, messageDto);

            await Clients.Groups(roomId.ToString()).SendAsync("GetNewMessage", messageDto.Sender, messageDto.Message, messageDto.Time.ToShortTimeString());
        }
        public override async Task OnConnectedAsync()
        {
            var roomId = await _chatRoomService.CreateChaRoom(Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

            await Clients.Caller.SendAsync("GetNewMessage", "Support from yoursite.com", "How can I help you?", DateTime.Now.ToShortTimeString().ToString());
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Connect support to group
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [Authorize]
        public async Task JoinRoom(Guid roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString()); 
        }

        [Authorize]
        public async Task LeaveRoom(Guid roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }


    }
}
