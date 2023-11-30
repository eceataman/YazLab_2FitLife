namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Target")]
    public partial class Target
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TargetId { get; set; }

        public int? UserId { get; set; }

        [StringLength(50)]
        public string Goal { get; set; }
    }
}
