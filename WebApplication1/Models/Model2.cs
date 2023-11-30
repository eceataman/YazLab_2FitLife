using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication1.Models
{
    public partial class Model2 : DbContext
    {
        public Model2()
            : base("name=Model2")
        {
        }

        public virtual DbSet<Coach> Coaches { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coach>()
                .Property(e => e.CoachName)
                .IsUnicode(false);

            modelBuilder.Entity<Coach>()
                .Property(e => e.CoachSurname)
                .IsUnicode(false);

            modelBuilder.Entity<Coach>()
                .Property(e => e.CoachExpert)
                .IsUnicode(false);

            modelBuilder.Entity<Coach>()
                .Property(e => e.CoachExperience)
                .IsUnicode(false);

            modelBuilder.Entity<Coach>()
                .Property(e => e.CoachMail)
                .IsUnicode(false);

            modelBuilder.Entity<Coach>()
                .Property(e => e.CoachPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Coach>()
                .Property(e => e.CoachPassword)
                .IsUnicode(false);
        }
    }
}
