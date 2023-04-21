using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataSubject _subject;
        public MessageRepository(DataSubject subject)
        {
            _subject = subject;
        }
        public void AddMessage(Message message)
        {
            _subject.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _subject.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _subject.Messages.FindAsync(id);
        }

        public Task<PagedList<MessageDto>> GetMessagesForUser()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _subject.SaveChangesAsync() > 0;
        }
    }
}