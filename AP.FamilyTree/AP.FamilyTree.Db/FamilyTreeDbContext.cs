using System;
using AP.FamilyTree.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace AP.FamilyTree.Db
{
    public class FamilyTreeDbContext : DbContext
    {
        public FamilyTreeDbContext(DbContextOptions<FamilyTreeDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder
            //    .Entity<ViewPackageDocument>(eb => { eb.HasNoKey(); })
        }
        //public DbSet<TreesModel> TreesDbSet { get; set; }
        //public DbSet<HumanModel> HumanDbSet { get; set; }
        //public DbSet<NodeModel> NodeDbSet { get; set; }
        DbSet<LogApplicationError> LogApplicationErrorDbset { get; set; }
    }
}
