using backend.Data;
using backend.Dtos;
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
							.OfType<RegisteredUser>()
							.Where(u => u.Name == username)
							.FirstOrDefaultAsync();
		}

		public async Task<RegisteredUser> LoginAsync(LoginRequest user)
		{
			var userT = await GetUserAsync(user.Username) as RegisteredUser ??
				throw new ArgumentException("User doesn't exist");

			if (userT.PasswordHash is null
				|| !_passwordService.VerifyPassword(userT.PasswordHash, user.Password))
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