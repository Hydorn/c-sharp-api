using Microsoft.EntityFrameworkCore;

namespace EntityFW.Services
{
    public class UserService
    {
        private readonly EntityFWContext _context;
        public UserService(EntityFWContext context)
        {
            this._context = context;
        }

        public async Task<bool> CreateUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }


        public async Task<User?> GetUserByEmail(string email)
        {
            // Use LINQ to find the user by email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }
    }
}
