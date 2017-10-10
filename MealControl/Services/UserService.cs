using Contexts;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;

namespace Services
{
    public class UserService
    {
        readonly MealEntities Entity = new MealEntities();

        public ICollection<DbValidationError> Validate(Parent user)
        {
            Entity.Parent.Add(user);
            var errors = new List<DbValidationError>();

            if (Entity.GetValidationErrors().Count() > 0)
                errors.AddRange(Entity.GetValidationErrors().First().ValidationErrors);

            if (user.Email != null)
            {
                var isValid = Regex.IsMatch(user.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (!isValid)
                    errors.Add(new DbValidationError("Email", "Digite um e-mail válido."));
            }

            if (user.Status.Id == 3 && Entity.Parent.Any(s => s.Email == user.Email))
                errors.Add(new DbValidationError("Email", "Já existe um usuário cadastrado com este email."));

            user.Students.ForEach(s =>
            {
                if (s.BirthDate == DateTime.MinValue || s.BirthDate.Date >= DateTime.Now.Date)
                    errors.Add(new DbValidationError("Birthdate", "Data nascimento inválida."));

                if (string.IsNullOrEmpty(s.Name))
                    errors.Add(new DbValidationError("Student Name ", "Nome do estudante é obrigatório."));

                if (s.Period == null)
                    errors.Add(new DbValidationError("Period", "Periodo é obrigatório."));
            });

            return errors;
        }

        public Parent Save(Parent parent)
        {
            //var entity = new MealEntities();
            var entity = new MealEntities();
            var model = new Parent();
            parent.Students.ForEach(s => s.Period = entity.Period.FirstOrDefault(p => p.Id == s.Period.Id));
            parent.Status = entity.Status.First(s => s.Id == parent.Status.Id);
            var erros = Validate(parent);

            if (parent.Id != 0)
            {
                model = entity.Parent.Find(parent.Id);
                entity.Entry(model).CurrentValues.SetValues(parent);                                

                //Entity.Parent.Attach(parent);
                //Entity.Entry(parent).State = System.Data.Entity.EntityState.Modified;                

                var errors = entity.GetValidationErrors().ToList();
                entity.SaveChanges();
                return model;
            }

            model = Entity.Parent.Add(parent);
            Entity.SaveChanges();
            return model;
        }

        public Parent GetUser(Parent parent)
        {
            return Entity.Parent
               .FirstOrDefault(x => x.Name.Contains((parent.Name != null ? parent.Name : x.Name)) &&
                           x.Email.Contains((parent.Email != null ? parent.Email : x.Email)));
        }

        public Parent GetUser(int id = 0)
        {
            return Entity.Parent.FirstOrDefault(s => s.Id == id);
        }

        public List<Parent> GetParents(Parent parent)
        {
            if (parent.Status != null)
            {
                if (parent.Status.Description == "Todos")
                {
                    return Entity.Parent
                          .Where(x => x.Name.Contains((parent.Name != null ? parent.Name : x.Name)) &&
                                      x.UserName.Contains((parent.UserName != null ? parent.UserName : x.UserName)) &&
                                      x.Email.Contains((parent.Email != null ? parent.Email : x.Email)))
                           .OrderBy(x => x.CreatedAt)
                           .ToList();
                }

                return Entity.Parent
                        .Where(x => x.Name.Contains((parent.Name != null ? parent.Name : x.Name)) &&
                                    x.UserName.Contains((parent.UserName != null ? parent.UserName : x.UserName)) &&
                                    x.Email.Contains((parent.Email != null ? parent.Email : x.Email)) &&
                                    x.Status.Description == (parent.Status.Description != null ? parent.Status.Description : x.Status.Description))
                        .OrderBy(x => x.CreatedAt)
                        .ToList();
            }

            return Entity.Parent
                        .Where(x => x.Name.Contains((parent.Name != null ? parent.Name : x.Name)) &&
                                    x.UserName.Contains((parent.UserName != null ? parent.UserName : x.UserName)) &&
                                    x.Email.Contains((parent.Email != null ? parent.Email : x.Email)))
                        .OrderBy(x => x.CreatedAt)
                        .ToList();
        }

        public List<Status> GetStatus()
        {
            return Entity.Status.ToList();
        }

        public List<UserType> GetTypes()
        {
            return Entity.Type.ToList();
        }
    }
}
