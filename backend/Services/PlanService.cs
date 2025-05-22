using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
	class PlanService(AppDbContext _db)
	{
		private static readonly Random _random = new();
		private static readonly int _codeSize = 16;

		private async Task<string> GeneratePlanCode()
		{
			char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
			char[] buffer = new char[_codeSize];
			string code;

			do
			{
				for (int i = 0; i < _codeSize; i++)
				{
					buffer[i] = chars[_random.Next(chars.Length)];
				}
				code = new string(buffer);
			} while (await CheckCode(code));

			return code;
		}

		public async Task<Boolean> CheckCode(string code)
		{
			return await _db.Plans.AnyAsync(p => p.Code == code);
		}

		public async Task<Plan?> GetPlanAsync(PlanRequest planR)
		{
			return await _db.Plans.Where(p => p.Code == planR.Code).FirstOrDefaultAsync();
		}

		public async Task<ICollection<User>> GetPlanUsersAsync(Plan plan)
		{
			return await _db.PlanUsers
				.Where(pu => pu.Plan == plan)
				.Select(pu => pu.User)
				.ToListAsync();
		}

		public async Task<Plan> CreatePlanAsync(PlanRequest planR)
		{
			planR.Code = await GeneratePlanCode();
			Plan plan = new()
			{
				Title = planR.Title,
				Code = planR.Code,
				Location = planR.Location,
				Time = planR.Time
			};
			
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