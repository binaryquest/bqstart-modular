using BinaryQuest.Framework.ModularCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bqStart.Data
{
    public class MainDataContext : DbContext
    {
        public MainDataContext(DbContextOptions<MainDataContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Department> Departments => Set<Department>();
        public DbSet<ExampleClass> ExampleClasses => Set<ExampleClass>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    }
}
