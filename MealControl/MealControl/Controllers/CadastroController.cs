using MealControl.Models;
using Models;
using Services;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MealControl.Controllers
{
    public class CadastroController : BaseController
    {
        const int STATUS_WAITING_EVALUATION = 3;        

        public UserService service = new UserService();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Load()
        {          
            return Json(new
            {         
                period = entity.Period.ToList()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(Parent parent)
        {
            entity = new Contexts.MealEntities();

            try
            {
                parent.Status = entity.Status.FirstOrDefault(s => s.Id == STATUS_WAITING_EVALUATION);
                var errors = service.Validate(parent);
                if (errors.Count() > 0)
                    return Json(errors);

                if (parent.Id != 0)
                    parent.Password = entity.Parent.First(s => s.Id == parent.Id).Password;
                else
                    parent.Password = BCrypt.Net.BCrypt.HashPassword(parent.Password);                

                parent = service.Insert(parent);                

                //Send email to all admins advising about the new user waiting for their evaluation
                var subject = "Novo usuário cadastrado - Aguardando liberação";
                var message = System.IO.File.ReadAllText(Server.MapPath("~/ViewsEmails/WaitingEvaluation.html")).Replace("##NAME##", parent.Name);
                var admins = entity.User.Where(s => s.Type.Id == 2).ToList();

                admins.ForEach(s =>
                {
                    var email = new Email { Mailto = s.Email, Subject = subject, Body = message };

                    try
                    {
                        new Mailer(email).Send();
                    }
                    catch (Exception ex)
                    {
                                               
                    }

                });

                return new HttpStatusCodeResult(HttpStatusCode.Created);//201                
            }
            catch (Exception ex)
            {
                //Something wrong was caught when trying to save user
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                    "Uma falha ocorreu ao salvar os dados do cadastro."); //500                
            }
        }
    }
}