namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Goal")]
    public partial class Goal
    {
        [Key]
        public int TargetId { get; set; }

        [StringLength(50)]
        public string TargetName { get; set; }
    }
}
