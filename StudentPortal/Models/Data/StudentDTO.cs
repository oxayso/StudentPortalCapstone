using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentPortal.Models.Data
{
    [Table("tblStudents")]
    public class StudentDTO
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Root { get; set; }
        public string CohortName { get; set; }
        public int CohortId { get; set; }
        public string ImageName { get; set; }

        [ForeignKey("CohortId")]
        public virtual CohortDTO Cohort { get; set; }
    }
}