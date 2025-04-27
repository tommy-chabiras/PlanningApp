using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
	class UserService(AppDbContext _db, PasswordService _passwordService)
	{
		public async Task<bool> CheckUsernameASync(string username)
		{
			return await _db.Users.AnyAsync(u => u.Name == username);
		}

		public async Task<User?> GetUserAsync(string username)
		{
			return await _db.Users
							.Where(u => u.Name.Equals(
								username, StringComparison.OrdinalIgnoreCase
							))
							.FirstOrDefaultAsync();
		}

		public async Task<bool> LoginAsync(string username, string password)
		{
			var user = await GetUserAsync(username) ??
				throw new ArgumentException("User doesn't exist");

			if (user.PasswordHash is null
				|| !_passwordService.VerifyPassword(password, user.PasswordHash))
			{
				throw new UnauthorizedAccessException("Password is Incorrect.");
			}

			return true;
		}
	}
}