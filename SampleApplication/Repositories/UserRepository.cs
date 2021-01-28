using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SampleApplication.Database;
using SampleApplication.DTOs;
using SampleApplication.Entities;
using SampleApplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApplication.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext databaseContext;
        private readonly IMapper mapper;

        public UserRepository(DatabaseContext databaseContext, IMapper mapper)
        {
            this.databaseContext = databaseContext;
            this.mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await databaseContext.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await databaseContext.Users
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await databaseContext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await databaseContext.Users
                .Include(x => x.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await databaseContext.Users
                .Include(x => x.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await databaseContext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            databaseContext.Entry(user).State = EntityState.Modified;
        }
    }
}