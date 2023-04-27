using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataSubject _subject;
        private readonly IMapper _mapper;
        public UserRepository(DataSubject subject, IMapper mapper)
        {
            _mapper = mapper;
            _subject = subject;

        }

        public async Task<MemberDto> GetMemberAsync(string username, bool isCurrentUser)
        {
            var query = _subject.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            if (isCurrentUser) query = query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _subject.Users
                .AsQueryable();

            query = query.Where(u => u.UserName != userParams.CurrentUsername);
            query = query.Where(u => u.Gender == userParams.Gender);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<MemberDto>.CreateAsync(query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _subject.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByPhotoId(int photoId)
        {
            return await _subject.Users
                .Include(p => p.Photos)
                .IgnoreQueryFilters()
                .Where(p => p.Photos.Any(p => p.Id == photoId))
                .FirstOrDefaultAsync();
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _subject.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<string> GetUserGender(string username)
        {
            return await _subject.Users.Where(x => x.UserName == username).Select(x => x.Gender).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _subject.Users
            .Include(p => p.Photos)
            .ToListAsync();
        }

        public void Update(AppUser user)
        {
            _subject.Entry(user).State = EntityState.Modified;
        }
    }
}