namespace backend.Models
{
	public class User
	{
		public required int Id { get; set; }
		public required string Name { get; set; }
		public string? PasswordHash { get; set; }
		public string? Email { get; set; }
		public bool IsAuthenticated { get; set; }

		public DateTime? LastLogin { get; set; }
	}
}