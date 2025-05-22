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
			return await _db.Users
								.OfType<RegisteredUser>()
								.AnyAsync(u => u.Username == username);
		}

		public async Task<bool> CheckEmailAsync(string email)
		{
			return await _db.Users
								.OfType<RegisteredUser>()
								.AnyAsync(u => u.Email == email);
		}

		public async Task<User?> GetUserAsync(string username)
		{
			return await _db.Users
								.OfType<RegisteredUser>()
								.Where(u => u.Username == username)
								.FirstOrDefaultAsync();
		}

		public async Task<User?> GetUserAsync(int id)
		{
			return await _db.Users
								.OfType<User>()
								.Where(u => u.Id == id)
								.FirstOrDefaultAsync();
		}

		public async Task<RegisteredUser> LoginAsync(LoginRequest r)
		{
			var userT = await GetUserAsync(r.Username) as RegisteredUser ??
				throw new ArgumentException("User doesn't exist");

			if (userT.PasswordHash is null
				|| !_passwordService.VerifyPassword(userT.PasswordHash, r.Password))
			{
				throw new UnauthorizedAccessException("Password is Incorrect.");
			}

			return userT;
		}

		public async Task<User> EditUserAsync(User user)
		{
			var userT = await GetUserAsync(user.Id) ??
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

		public async Task<RegisteredUser> AddUserAsync(SignupRequest r)
		{


			if (await CheckUsernameAsync(r.Username))
			{
				throw new ArgumentException("The selected username is taken.");
			}
			else if (await CheckEmailAsync(r.Email))
			{
				throw new ArgumentException("An account with this email already exists.");
			}
 
			var user = new RegisteredUser
			{
				Name = r.Name,
				Username = r.Username,
				PasswordHash = _passwordService.HashPassword(r.Password),
				Email = r.Email,
				IsVerified = false
			};

			await _db.Users.AddAsync(user);
			await _db.SaveChangesAsync();
			return user;
		}

		public async Task<GuestUser> AddUserAsync(SignupRequestGuest r)
		{
			GuestUser user = new()
			{
				Name = r.Name
			};

			await _db.Users.AddAsync(user);
			await _db.SaveChangesAsync();
			return user;
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