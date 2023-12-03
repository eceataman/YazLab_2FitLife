namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Coach")]
    public partial class Coach
    {
        public int CoachId { get; set; }

        [StringLength(50)]
        public string CoachName { get; set; }

        [StringLength(50)]
        public string CoachSurname { get; set; }

        [StringLength(50)]
        public string CoachExperience { get; set; }

        [StringLength(50)]
        public string CoachMail { get; set; }

        [StringLength(50)]
        public string CoachPhone { get; set; }

        [StringLength(50)]
        public string CoachPassword { get; set; }

        public int? CoachQuota { get; set; }

        public int? TargetId { get; set; }
        public List<User> AssignedUsers { get; set; }
    }
}
