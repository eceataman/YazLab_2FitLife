namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Progress")]
    public partial class Progress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        public double? UserHeight { get; set; }

        public double? UserWeight { get; set; }

        public double? UserFat { get; set; }

        public double? UserMuscle { get; set; }

        public double? UserBMI { get; set; }
    }
}
