using Contexts;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data.Entity.Migrations;

namespace Services
{
    public class UserService
    {
        readonly MealEntities Entity = new MealEntities();

        public ICollection<DbValidationError> Validate(Parent parent)
        {
            Entity.Parent.Add(parent);
            var errors = new List<DbValidationError>();

            if (Entity.GetValidationErrors().Count() > 0)
                errors.AddRange(Entity.GetValidationErrors().First().ValidationErrors);

            if (parent.Email != null)
            {
                var isValid = Regex.IsMatch(parent.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (!isValid)
                    errors.Add(new DbValidationError("Email", "Digite um e-mail válido."));
            }

            if (parent.Status.Id == 3 && Entity.Parent.Any(s => s.Email == parent.Email))
                errors.Add(new DbValidationError("Email", "Já existe um usuário cadastrado com este email."));

            parent.Students.ForEach(s =>
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

        public Parent Insert(Parent parent)
        {
            var model = Entity.Parent.Add(parent);
            Entity.SaveChanges();
            return model;
        }

        public Parent Update(Parent parent)
        {
            var model = Entity.Parent.Find(parent.Id);
            var _parent = new Parent
            {
                Id = parent.Id,
                CreatedAt = parent.CreatedAt,
                Name = parent.Name,
                UserName = parent.UserName,
                Password = parent.Password,
                Email = parent.Email,
                Phone = parent.Phone,
                Status = Entity.Status.Find(parent.Status.Id),
                Students = parent.Students
            };

            //Entity.Parent.AddOrUpdate(_parent);
            //Entity.SaveChanges();

            //Entity.Parent.Add(_parent);
            //Entity.Entry(_parent).State = System.Data.Entity.EntityState.Modified;
            //Entity.SaveChanges();            

            //Entity.Entry(model).CurrentValues.SetValues(_parent);
            //Entity.SaveChanges();
            
            return _parent;
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
            if (parent.Status == null)
            {
                return Entity.Parent
                            .Where(x => x.Name.Contains((parent.Name != null ? parent.Name : x.Name)) &&
                                        x.UserName.Contains((parent.UserName != null ? parent.UserName : x.UserName)) &&
                                        x.Email.Contains((parent.Email != null ? parent.Email : x.Email)))
                            .OrderBy(x => x.CreatedAt)
                            .ToList();
            }

            if (parent.Status.Description.ToLower() == "todos")
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
