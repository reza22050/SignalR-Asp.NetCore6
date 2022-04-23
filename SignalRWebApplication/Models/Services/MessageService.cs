using SignalRWebApplication.Context;
using SignalRWebApplication.Models.Entities;

namespace SignalRWebApplication.Models.Services
{
    public class MessageService : IMessageService
    {
        private readonly DataBaseContext _context;
        public MessageService(DataBaseContext context)
        {
            _context = context;
        }
        public Task<List<MessageDto>> GetChatMessage(Guid RoomId)
        {
            var message = _context.ChatMessages.Where(x => x.ChatRoomId == RoomId)
                .Select(x => new MessageDto()
                {
                    Message = x.Message,
                    Sender = x.Sender,
                    Time = x.Time
                })
                .OrderBy(x => x.Time).ToList();
            return Task.FromResult(message);
        }

        public Task SaveChatMessage(Guid RoomId, MessageDto message)
        {
            var room = _context.ChatRooms.SingleOrDefault(x=>x.Id == RoomId);
            ChatMessage chatMessage = new ChatMessage()
            {
                ChatRoom = room,
                Message = message.Message,
                Sender = message.Sender,
                Time = message.Time,
            };
            _context.Add(chatMessage);
            _context.SaveChanges();
            return Task.CompletedTask;
        
                
        }
    }

}
