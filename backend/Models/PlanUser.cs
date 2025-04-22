namespace backend.Models
{
    public class PlanUser
    {
        public required int Id { get; set; }
        public required int PlanId { get; set; }
        public required int UserId { get; set; }
        
        public Role Role { get; set; } = Role.Participant;

        public required Plan Plan { get; set; }
        public required User User { get; set; }
    }
}