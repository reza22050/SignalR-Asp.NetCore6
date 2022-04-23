namespace SignalRWebApplication.Models.Services
{
    public interface IMessageService
    {
        Task SaveChatMessage(Guid RoomId, MessageDto message);
        Task<List<MessageDto>> GetChatMessage(Guid RoomId);
    }

}
