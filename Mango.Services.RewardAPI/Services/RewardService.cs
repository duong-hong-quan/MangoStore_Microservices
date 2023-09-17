
using Mango.Services.EmailAPI.Services;
using Mango.Services.RewardAPI.Data;
using Mango.Services.RewardAPI.Message;
using Mango.Services.RewardAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.RewardAPI.Services
{
    public class RewardService : IRewardService
    {

        private DbContextOptions<AppDBContext> _dbOptions;

        public RewardService(DbContextOptions<AppDBContext> dbOptions)
        {
            this._dbOptions = dbOptions;
        }

       

        public async Task UpdateRewards(RewardsMessage message)
        {
            try
            {
                Rewards rewards = new()
                {
                    OrderId = message.OrderId,
                    RewardsActivity = message.RewardsActivity,
                    UserId = message.UserId,
                    RewardsDate = DateTime.Now
                };
                await using var _db = new AppDBContext(_dbOptions);
                _db.Rewards.Add(rewards);
                await _db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
            }
        }
    }
}
