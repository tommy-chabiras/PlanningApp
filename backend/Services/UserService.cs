using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
	class UserService(AppDbContext _db, PasswordService _passwordService)
	{
		public async Task<bool> CheckUsernameAsync(string username)
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

		public async Task<User> LoginAsync(User user)
		{
			var userT = await GetUserAsync(user.Name) ??
				throw new ArgumentException("User doesn't exist");

			if (userT.PasswordHash is null
				|| !_passwordService.VerifyPassword(user.PasswordHash!, userT.PasswordHash))
			{
				throw new UnauthorizedAccessException("Password is Incorrect.");
			}

			return userT;
		}

		public async Task<User> EditUserAsync(User user)
		{
			var userT = await GetUserAsync(user.Name) ??
				throw new ArgumentException("User doesn't exist");
			_db.Users.Update(user);
			await _db.SaveChangesAsync();
			return userT;
		}

		public async Task DeleteUserAsync(User user)
		{
			_db.Users.Remove(user);
			await _db.SaveChangesAsync();
		}

		public async Task AddUserAsync(User user)
		{
			if (await CheckUsernameAsync(user.Name))
			{
				throw new ArgumentException("Username taken.");
			}

			await _db.Users.AddAsync(user);
			await _db.SaveChangesAsync();
		}

		public async Task<ICollection<Plan>> GetUserPlansAsync(User user)
		{
			return await _db.PlanUsers
				.Where(pu => pu.User == user)
				.Select(pu => pu.Plan)
				.ToListAsync();
		}
	}
}