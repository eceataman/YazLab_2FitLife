using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication1.Models
{
    public partial class Model3 : DbContext
    {
        public Model3()
            : base("name=Model31")
        {
        }

        public virtual DbSet<Goal> Goal { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Goal>()
                .Property(e => e.TargetName)
                .IsUnicode(false);
        }
    }
}
