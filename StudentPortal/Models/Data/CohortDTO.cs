using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentPortal.Models.Data
{
    [Table("tblCohorts")]

    public class CohortDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Root { get; set; }
        public int Sorting { get; set; }
    }
}