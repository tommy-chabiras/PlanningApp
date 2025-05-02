namespace backend.Models
{
	public class User
	{
		public required int Id { get; set; }
		public required string Name { get; set; }

		public DateTime? LastLogin { get; set; }
	}

	public class RegisteredUser : User
	{
		public required string PasswordHash { get; set; }
		public required string Email { get; set; }
		public bool IsVerified { get; set; }
	}

	public class GuestUser : User
	{

	}
}