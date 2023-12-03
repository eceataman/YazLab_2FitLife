namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NutritionPlan")]
    public partial class NutritionPlan
    {
        [Key]
        public int NutritionId { get; set; }

        public int? CalorieTarget { get; set; }

        [StringLength(50)]
        public string Breakfast { get; set; }

        [StringLength(50)]
        public string Snack { get; set; }

        [StringLength(50)]
        public string Lunch { get; set; }

        [StringLength(50)]
        public string Dinner { get; set; }

        public int? UserId { get; set; }
    }
}
