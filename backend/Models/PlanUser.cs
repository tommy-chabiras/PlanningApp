namespace backend.Models
{
	public class PlanUser
	{
		public int Id { get; set; }
		public required int PlanId { get; set; }
		public required int UserId { get; set; }

		public required Role Role { get; set; } = Role.Participant;

		public Plan Plan { get; set; } = default!;
		public User User { get; set; } = default!;
	}
}