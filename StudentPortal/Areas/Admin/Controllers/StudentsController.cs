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

        [HttpPost]
        public void ReorderCohorts(int[] id)
        {
            using (Db db = new Db())
            {
                int count = 1;

                CohortDTO dto;

                foreach (var catId in id)
                {
                    dto = db.Cohort.Find(catId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }
            }

        }

        public ActionResult DeleteCohort(int id)
        {
            using (Db db = new Db())
            {
                CohortDTO dto = db.Cohort.Find(id);

                db.Cohort.Remove(dto);

                db.SaveChanges();
            }

            return RedirectToAction("Cohorts");
        }

        [HttpPost]
        public string RenameCohort(string newCatName, int id)
        {
            using (Db db = new Db())
            {
                if (db.Cohort.Any(x => x.Name == newCatName))
                    return "titletaken";

                CohortDTO dto = db.Cohort.Find(id);

                dto.Name = newCatName;
                dto.Root = newCatName.Replace(" ", "-").ToLower();

                db.SaveChanges();
            }

            return "test";
        }

    }

}