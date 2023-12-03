using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication1.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model19")
        {
        }

        public virtual DbSet<Coach> Coaches { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coach>()
                .Property(e => e.CoachName)
                .IsUnicode(false);

            modelBuilder.Entity<Coach>()
                .Property(e => e.CoachSurname)
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

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserSurname)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserPass)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserMail)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserGender)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserPhone)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserPhoto)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.SecurityQuestion)
                .IsUnicode(false);
        }
    }
}
