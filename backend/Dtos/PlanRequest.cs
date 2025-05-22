namespace backend.Dtos
{
	public class PlanRequest
	{
		public required string Title { get; set; }
		public string? Code { get; set; }
		public string? Description { get; set; }
		public required string Location { get; set; }
		public required DateTime Time { get; set; }
	}
}