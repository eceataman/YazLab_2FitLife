namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public int UserId { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string UserSurname { get; set; }

        [StringLength(250)]
        public string UserPass { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UserBirth { get; set; }

        [StringLength(50)]
        public string UserMail { get; set; }

        [StringLength(50)]
        public string UserGender { get; set; }

        [StringLength(50)]
        public string UserPhone { get; set; }

        [StringLength(50)]
        public string UserPhoto { get; set; }

        public int? UserRole { get; set; }

        [StringLength(50)]
        public string SecurityQuestion { get; set; }

        public int? RecordStatus { get; set; }

        public int? TargetId { get; set; }

        public int? CoachId { get; set; }
    }
}
