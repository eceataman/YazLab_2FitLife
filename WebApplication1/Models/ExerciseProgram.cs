namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExerciseProgram")]
    public partial class ExerciseProgram
    {
        [Key]
        public int ExerciseId { get; set; }

        [StringLength(50)]
        public string ExerciseName { get; set; }

        public int? SetNumber { get; set; }

        public int? RepeatNumber { get; set; }

        [StringLength(250)]
        public string VideoGuide { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartingDate { get; set; }

        public int? ProgramDuration { get; set; }

        public int? UserId { get; set; }
    }
}
