using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
	class PlanService(AppDbContext _db)
	{
		public async Task<ICollection<Plan>> GetUserPlansAsync(User user)
		{
			return await _db.PlanUsers
				.Where(pu => pu.User == user)
				.Select(pu => pu.Plan)
				.ToListAsync();
		}

		public async Task<ICollection<User>> GetPlanUsersAsync(Plan plan)
		{
			return await _db.PlanUsers
				.Where(pu => pu.Plan == plan)
				.Select(pu => pu.User)
				.ToListAsync();
		}

		public async Task<Plan> CreatePlanAsync(Plan plan)
		{
			await _db.Plans.AddAsync(plan);
			await _db.SaveChangesAsync();
			return plan;
		}

		public async Task<Plan> EditPlanAsync(Plan plan)
		{
			await _db.Plans.FindAsync(plan.Id);
			await _db.SaveChangesAsync();
			return plan;
		}

		public async Task DeletePlanAsync(Plan plan)
		{
			_db.Plans.Remove(plan);
			await _db.SaveChangesAsync();
		}
	}
}