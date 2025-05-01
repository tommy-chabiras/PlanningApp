namespace backend.Models
{
	public class Plan
	{
		public int Id { get; set; }
		public required string Code { get; set; }
		public required string Title { get; set; }
		public string? Description { get; set; }
		public required string Location { get; set; }
		public DateTime Time { get; set; }

		// foreign keys
		public ICollection<PlanUser> Participants { get; set; } = [];
	}
}