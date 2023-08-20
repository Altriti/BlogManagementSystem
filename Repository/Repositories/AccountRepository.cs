using Domain.DTOs;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.DataContext;

namespace Repository.Repositories
{
    public interface IAccountRepository
    {
        Task<AppUser> FindUser(LoginDto loginDto);
        Task CreateUser(AppUser user);
        void RemoveUserFromDatabase(AppUser user);
        AppUser FindUserById(string id);
        IdentityUserRole<string> RoleUser(string id);
        void SaveChanges();

    }


    public class AccountRepository : IAccountRepository
    {
        private readonly MyDataContext context;

        public AccountRepository(MyDataContext context)
        {
            this.context = context;
        }

        public Task<AppUser> FindUser(LoginDto loginDto)
        {
            var user = this.context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            return user;
        }

        public async Task CreateUser(AppUser user)
        {
            await this.context.Users.AddAsync(user);
        }

        public void RemoveUserFromDatabase(AppUser user)
        {
            this.context.Users.Remove(user);
        }

        public AppUser FindUserById(string id)
        {
            return this.context.Users.FirstOrDefault(u => u.Id == id);
        }

        public IdentityUserRole<string> RoleUser(string id)
        {
            return this.context.UserRoles.FirstOrDefault(ru => ru.UserId == id);
        }

        public void SaveChanges()
        {

            var result = this.context.SaveChanges() > 0;

            if (!result)
            {
                throw new System.Exception($"Failed to complete task");
            }
        }
    }
}
