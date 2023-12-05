using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication1.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model110")
        {
        }

        public virtual DbSet<Message> Messages { get; set; }
        //public virtual DbSet<Progress> Progresses { get; set; }
        public virtual DbSet<Coach> Coaches { get; set; }
        public virtual DbSet<ExerciseProgram> ExercisePrograms { get; set; }
        public virtual DbSet<NutritionPlan> NutritionPlans { get; set; }
        public virtual DbSet<Target> Targets { get; set; }
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

            modelBuilder.Entity<ExerciseProgram>()
                .Property(e => e.ExerciseName)
                .IsUnicode(false);

            modelBuilder.Entity<ExerciseProgram>()
                .Property(e => e.VideoGuide)
                .IsUnicode(false);

            modelBuilder.Entity<NutritionPlan>()
                .Property(e => e.Breakfast)
                .IsUnicode(false);

            modelBuilder.Entity<NutritionPlan>()
                .Property(e => e.Snack)
                .IsUnicode(false);

            modelBuilder.Entity<NutritionPlan>()
                .Property(e => e.Lunch)
                .IsUnicode(false);

            modelBuilder.Entity<NutritionPlan>()
                .Property(e => e.Dinner)
                .IsUnicode(false);

            modelBuilder.Entity<Target>()
                .Property(e => e.Goal)
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
