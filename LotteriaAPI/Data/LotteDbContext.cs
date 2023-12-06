using LotteriaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace LotteriaAPI.Data
{
    public class LotteDbContext : DbContext
    {
        public LotteDbContext(DbContextOptions options) : base(options)
        {

        }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Register>().Property(b => b.Role).HasDefaultValue(1);
            modelBuilder.Entity<CartDetail>().HasKey(op => new { op.CartId, op.ProductId });
            modelBuilder.Entity<CartDetail>().HasOne(op => op.Cart).WithMany(o => o.CartDetails).HasForeignKey(op => op.CartId);
            modelBuilder.Entity<CartDetail>().HasOne(op => op.Product).WithMany(p => p.CartDetails).HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<Cart>().HasOne(e => e.Register).WithOne(e => e.Cart).HasForeignKey<Cart>(e => e.UserId).IsRequired();

            modelBuilder.Entity<Register>().HasMany(r => r.Bills).WithOne(r => r.Register).HasForeignKey(r => r.UserId).IsRequired();
            modelBuilder.Entity<BillDetail>().HasKey(bu => new { bu.BillId, bu.ProductId });
            modelBuilder.Entity<BillDetail>().HasOne(bu => bu.Bill).WithMany(b => b.BillDetails).HasForeignKey(b => b.BillId);
            modelBuilder.Entity<BillDetail>().HasOne(bu => bu.Product).WithMany(u => u.BillDetails).HasForeignKey(u => u.ProductId);
    }

        //public DbSet<Register> Registers { get; set; }
        public DbSet<Register> Register { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; } 
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }

        public DbSet<News> News { get; set; }

        


    }
}
