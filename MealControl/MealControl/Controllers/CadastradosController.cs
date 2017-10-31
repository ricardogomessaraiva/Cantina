using Contexts;
using MealControl.Models;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MealControl.Controllers
{
    public class CadastradosController : Controller
    {
        const int STATUS_ACTIVE = 1;
        const int USER_TYPE_DEVELOPER = 1;
        const int USER_TYPE_ADMIN = 2;

        public UserService service = new UserService();

        // GET: Cadastrados
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Status()
        {
            List<Status> status = service.GetStatus();
            return Json(new
            {
                status = status
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Search(Parent parent)
        {
            List<Parent> parents = service.GetParents(parent);

            return Json(new
            {
                parentList = parents
            });
        }

        [HttpPost]
        public ActionResult Update(Parent parent)
        {
            var _parent = new Parent();
            List<DbValidationError> errors = service.Validate(parent);

            if (errors.Count != 0)
            {
                return Json(new
                {
                    errors = errors,
                    parent = _parent
                });
            }

            _parent = service.Update(parent);

            //If the parent status is active must send an email advising about it
            if (_parent.Status.Id == STATUS_ACTIVE)
            {
                var subject = "Acesso liberado ao Portal Vovó Chiquita. Uhuuul.";
                string message = System.IO.File.ReadAllText(Server.MapPath("~/ViewsEmails/AccessGranted.html")).Replace("##PARENT-NAME##", parent.Name);
                List<User> admins = new MealEntities().User.Where(s => s.Type.Id == USER_TYPE_ADMIN).ToList();

                var email = new Email { Mailto = _parent.Email, Subject = subject, Body = message };

                try
                {
                    new Mailer(email).Send();
                }
                catch (Exception ex)
                {
                    var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                    errors.Add(new DbValidationError(
                        "Erro no envio do email", "O status do usuário foi alterado com sucesso. Porém o erro abaixo ocorreu ao enviar o e-mail ao usuário."));

                    errors.Add(new DbValidationError(
                        "Mensagem do erro", msg));
                }
            }

            return Json(new
            {
                errors = errors,
                parent = _parent
            });
        }
    }
}