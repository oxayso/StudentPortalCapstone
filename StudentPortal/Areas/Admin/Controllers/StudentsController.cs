using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentPortal.Models.Data;
using StudentPortal.Models.ViewModels.Students;

namespace StudentPortal.Areas.Admin.Controllers
{
    public class StudentsController : Controller
    {
        public ActionResult Cohorts()
        {
            List<CohortVM> cohortVMList;

            using (Db db = new Db())
            {
                cohortVMList = db.Cohort
                    .ToArray()
                    .OrderBy(x => x.Sorting)
                    .Select(x => new CohortVM(x))
                    .ToList();
            }

            return View(cohortVMList);
        }

        [HttpPost]
        public string AddNewCohort(string catName)
        {
            string id;

            using (Db db = new Db())
            {
                if (db.Cohort.Any(x => x.Name == catName))
                    return "titletaken";

                CohortDTO dto = new CohortDTO();

                dto.Name = catName;
                dto.Root = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;

                db.Cohort.Add(dto);
                db.SaveChanges();

                id = dto.Id.ToString();
            }

            return id;
        }
    }

}