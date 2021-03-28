using System;
using System.Collections.Generic;
using System.Linq;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Db.Views;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
            modelBuilder
                .Entity<ViewName>(eb => { eb.HasNoKey(); });
        }
        public DbSet<TreesModel> TreesDbSet { get; set; }
        public DbSet<UserTree> UserTreeDbSet { get; set; }
        public DbSet<Access> AccessDbSet { get; set; }
        public DbSet<HumanModel> HumanDbSet { get; set; }
        public DbSet<NodeModel> NodeDbSet { get; set; }
        public DbSet<ViewUserModel> ViewUserModelDbSet { get; set; }
        public DbSet<ViewName> ViewNameDbSet { get; set; }
        DbSet<LogApplicationError> LogApplicationErrorDbset { get; set; }

        public List<string> GetAllRolesSort()
        {
            var sql = "select Name from AspNetRoles Order by Name";
            var result = ViewNameDbSet.FromSqlRaw(sql).ToList();
            return result?.Select(x=> x.Name).ToList();
        }
    }
}
