using Microsoft.EntityFrameworkCore;
using SignalRWebApplication.Context;
using SignalRWebApplication.Models.Entities;

namespace SignalRWebApplication.Models.Services
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly DataBaseContext _context;
        public ChatRoomService(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Guid> CreateChaRoom(string ConnectionId)
        {
            var existChatroom = _context.ChatRooms.SingleOrDefault(x => x.ConnectionId == ConnectionId);

            if (existChatroom != null)
            {
                return await Task.FromResult(existChatroom.Id);
            }
            
            ChatRoom chatRoom = new ChatRoom()
            {
                ConnectionId = ConnectionId,
                Id = Guid.NewGuid()
            };
            _context.Add(chatRoom);
            _context.SaveChanges();
            
            return await  Task.FromResult(chatRoom.Id);
        }

        public async Task<List<Guid>> GetAllRooms()
        {
            var rooms = _context.ChatRooms
                .Include(x=>x.ChatMessages)
                .Where(x=>x.ChatMessages.Any())
                .Select(x => x.Id).ToList();
            return await Task.FromResult(rooms);
        }

        public async Task<Guid> GetChatRoomForConnection(string ConnectionId)
        {
            var chatRoom = _context.ChatRooms.SingleOrDefault(x=> x.ConnectionId == ConnectionId);
            return await Task.FromResult(chatRoom.Id);
        }
    }
}
