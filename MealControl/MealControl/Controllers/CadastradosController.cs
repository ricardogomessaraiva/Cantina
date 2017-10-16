using Contexts;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MealControl.Controllers
{
    public class CadastradosController : Controller
    {
        public UserService service = new UserService();

        // GET: Cadastrados
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Status()
        {
            return Json(new
            {
                status = service.GetStatus()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Search(Parent parent)
        {
            var parents = service.GetParents(parent);
            if (parents.Count == 0)
                parents = new MealEntities().Parent
                    .Where(_parent => parent.Name.Contains(_parent.Students.FirstOrDefault(s => parent.Name.Contains(s.Name)).Name))
                    .OrderBy(x => x.CreatedAt)
                    .ToList();

            return Json(new
            {
                parentList = parents
            });
        }

        [HttpPost]
        public ActionResult Update(Parent parent)
        {
            //parent.Students.ForEach(s => { s.BirthDate = DateTime.Now.AddYears(-2); });
            var _parent = new Parent();
            var errors = service.Validate(parent);
            if (errors.Count == 0)
                _parent = service.Update(parent);
            
            return Json(new
            {
                errors = errors,
                parent = _parent
            });
        }
    }
}