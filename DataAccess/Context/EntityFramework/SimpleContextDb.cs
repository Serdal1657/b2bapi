using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context.EntityFramework
{
    public class SimpleContextDb : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=DESKTOP-PL1GA65;Database=B2bDb;Integrated Security=true;");

            optionsBuilder.UseSqlServer("Server=77.245.159.27\\MSSQLSERVER2019; Database=umitsizs_; User Id=umitsizserdal ; Password=Sb3713661.;");
        
//
//
        }

        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<EmailParameter> EmailParameters { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomersRelationship> CustomersRelationships { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PriceListDetail> PriceListDetails { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<Product> Products { get; set; }
        



    }
}
