using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentPortal.Models.Data;

namespace StudentPortal.Models.ViewModels.Students
{
    public class CohortVM
    {
        public CohortVM()
        {
        }

        public CohortVM(CohortDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Root = row.Root;
            Sorting = row.Sorting;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Root { get; set; }
        public int Sorting { get; set; }
    }
}
