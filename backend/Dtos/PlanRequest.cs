namespace backend.Dtos
{
	public class PlanRequest
	{
		public string? Code { get; set; }
		public required string Title { get; set; }
		public required string Location { get; set; }
		public required DateTime Time { get; set; }
		public string? Description { get; set; }
	}
}