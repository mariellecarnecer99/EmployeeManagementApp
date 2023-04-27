using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly DataSubject _subject;
        public UnitOfWork(DataSubject subject, IMapper mapper)
        {
            _subject = subject;
            _mapper = mapper;

        }
        public IUserRepository UserRepository => new UserRepository(_subject, _mapper);

        public IMessageRepository MessageRepository => new MessageRepository(_subject, _mapper);

        public ILikesRepository LikesRepository => new LikesRepository(_subject);

        public async Task<bool> Complete()
        {
            return await _subject.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _subject.ChangeTracker.HasChanges();
        }
    }
}