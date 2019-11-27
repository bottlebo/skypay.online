using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkyPay.Models;

namespace SkyPay.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Agent> Agents { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductProps> ProductProps { get; set; }
        public DbSet<CategoryProps> CategoryProps { get; set; }
        public DbSet<CategoryPropsValues> CategoryPropsValues { get; set; }
        public DbSet<ProductSet> ProductsSets { get; set; }
        public DbSet<ProductSetItem> ProductsSetItems { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentItem> DocumentItems { get; set; }
        public DbSet<ProductInStock> ProductsInStock { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

    }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
