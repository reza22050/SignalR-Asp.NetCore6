namespace SignalRWebApplication.Models.Services
{
    public interface IChatRoomService
    {
        Task<Guid> CreateChaRoom(string ConnectionId);
        Task<Guid> GetChatRoomForConnection(string ConnectionId);

        Task<List<Guid>> GetAllRooms();
    }
}
