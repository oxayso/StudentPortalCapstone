using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using StudentPortal.Models.Data;
using System.Web.Mvc;

namespace StudentPortal.Models.ViewModels.Students
{
    public class StudentVM
    {
        public StudentVM()
        {
        }

        public StudentVM(StudentDTO row)
        {
            Id = row.Id;
            FirstName = row.FirstName;
            LastName = row.LastName;
            Root = row.Root;
            CohortName = row.CohortName;
            CohortId = row.CohortId;
            ImageName = row.ImageName;
        }

        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Root { get; set; }
        public string CohortName { get; set; }
        public int CohortId { get; set; }
        public string ImageName { get; set; }

        public IEnumerable<SelectListItem> Cohorts { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}
