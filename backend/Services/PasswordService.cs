using Microsoft.AspNetCore.Identity;

namespace backend.Services
{
	class PasswordService
	{
		public readonly PasswordHasher<object> _hash = new();

		public string HashPassword(string pw)
		{
			return _hash.HashPassword((object)null!, pw);
		}

		public bool VerifyPassword(string hash, string pw)
		{
			return _hash.VerifyHashedPassword((object)null!, hash, pw) == PasswordVerificationResult.Success;
		}
	}
}