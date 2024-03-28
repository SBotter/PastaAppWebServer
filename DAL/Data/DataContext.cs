using BL.Models;
using DAL.FluentConfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
    
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products {  get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<ProductPackage> ProductPackages { get; set; }
        public DbSet<ProductIngredient> ProductIngredients { get; set; }
        public DbSet<ProductCookInstruction> ProductCookInstructions { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<CookInstruction> CookInstructions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new FluentCompany());
            modelBuilder.ApplyConfiguration(new FluentProduct());
            modelBuilder.ApplyConfiguration(new FluentCategory());
            modelBuilder.ApplyConfiguration(new FluentProductCategory());
            modelBuilder.ApplyConfiguration(new FluentProductPicture());
            modelBuilder.ApplyConfiguration(new FluentProductPackage());
            modelBuilder.ApplyConfiguration(new FluentIngredient());
            modelBuilder.ApplyConfiguration(new FluentProductIngredient());
            modelBuilder.ApplyConfiguration(new FluentProductCookInstruction());
            modelBuilder.ApplyConfiguration(new FluentCookInstruction());
        }
    }
}
