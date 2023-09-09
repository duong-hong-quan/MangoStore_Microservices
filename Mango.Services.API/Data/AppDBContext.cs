using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {

                CounponId = 1,
                CouponCode = "COCLE",
                DiscountAmount = 10,
                MinAmount = 20
            }
            );
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {

                CounponId = 2,
                CouponCode = "MONNE",
                DiscountAmount = 20,
                MinAmount = 40
            }
           );
        }
    }
}
