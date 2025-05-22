namespace backend.Dtos
{
	public class SignupRequest
	{
		public required string Name { get; set; }
		public required string Username { get; set; }
		public required string Password { get; set; }
		public required string Email { get; set; }
	}


	public class SignupRequestGuest
	{
		public required string Name { get; set; }
	}
}