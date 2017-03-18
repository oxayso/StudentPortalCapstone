using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
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
                cohortVMList = db.Cohorts
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
                if (db.Cohorts.Any(x => x.Name == catName))
                    return "titletaken";

                CohortDTO dto = new CohortDTO();

                dto.Name = catName;
                dto.Root = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;

                db.Cohorts.Add(dto);
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
                    dto = db.Cohorts.Find(catId);
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
                CohortDTO dto = db.Cohorts.Find(id);

                db.Cohorts.Remove(dto);

                db.SaveChanges();
            }

            return RedirectToAction("Cohorts");
        }

        [HttpPost]
        public string RenameCohort(string newCatName, int id)
        {
            using (Db db = new Db())
            {
                if (db.Cohorts.Any(x => x.Name == newCatName))
                    return "titletaken";

                CohortDTO dto = db.Cohorts.Find(id);

                dto.Name = newCatName;
                dto.Root = newCatName.Replace(" ", "-").ToLower();

                db.SaveChanges();
            }

            return "test";
        }

        [HttpGet]
        public ActionResult AddStudent()
        {
            StudentVM model = new StudentVM();

            using (Db db = new Db())
            {
                model.Cohorts = new SelectList(db.Cohorts.ToList(), "Id", "Name");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddStudent(StudentVM model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                using (Db db = new Db())
                {
                    model.Cohorts = new SelectList(db.Cohorts.ToList(), "Id", "Name");
                    return View(model);
                }
            }

            using (Db db = new Db())
            {
                if (db.Student.Any(x => x.FirstName == model.FirstName))
                {
                    model.Cohorts = new SelectList(db.Cohorts.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "Sorry! That student name is taken!");
                    return View(model);
                }
            }

            int id;

            using (Db db = new Db())
            {
                StudentDTO student = new StudentDTO();

                student.FirstName = model.FirstName;
                student.LastName = model.LastName;
                student.Root = model.FirstName.Replace(" ", "-").ToLower();
                student.CohortId = model.CohortId;

                CohortDTO catDTO = db.Cohorts.FirstOrDefault(x => x.Id == model.CohortId);
                student.CohortName = catDTO.Name;

                db.Student.Add(student);
                //db.SaveChanges();

                id = student.Id;
            }

            TempData["SM"] = "Successfully added a student!";

            #region Upload Image

            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);

            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);

            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);

            if (file != null && file.ContentLength > 0)
            {
                string ext = file.ContentType.ToLower();

                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        model.Cohorts = new SelectList(db.Cohorts.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "ERROR: The image was not uploaded - wrong image extension.");
                        return View(model);
                    }
                }

                string imageName = file.FileName;

                using (Db db = new Db())
                {
                    StudentDTO dto = db.Student.Find(id);
                    if (dto != null) dto.ImageName = imageName;

                    db.SaveChanges();
                }

                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                file.SaveAs(path);

                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }

            #endregion

            return RedirectToAction("AddStudent");
        }

        public ActionResult Students(int? page, int? catId)
        {
            List<StudentVM> listOfStudentVM;

            var pageNumber = page ?? 1;

            using (Db db = new Db())
            {
                listOfStudentVM = db.Student.ToArray()
                                  .Where(x => catId == null || catId == 0 || x.CohortId == catId)
                                  .Select(x => new StudentVM(x))
                                  .ToList();

                ViewBag.Categories = new SelectList(db.Cohorts.ToList(), "Id", "Name");

                ViewBag.SelectedCat = catId.ToString();
            }

            var onePageOfProducts = listOfStudentVM.ToPagedList(pageNumber, 3);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View(listOfStudentVM);
        }

    }

}