using Contexts;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data.Entity.Migrations;
using System.Data.Entity;

namespace Services
{
    public class UserService
    {
        const int STATUS_EVERYONE = 0;//Do not existe at database, just to ignore as a status in a filter
        const int STATUS_ACTIVE = 1;
        const int STATUS_INACTIVE = 2;
        const int STATUS_WAITING_EVALUATION = 3;
        const int STATUS_BLOCKED = 4;

        readonly MealEntities Entity = new MealEntities();

        public ICollection<DbValidationError> Validate(Parent parent)
        {
            if (parent.Students.Count == 0)
                parent.Students = null;

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

            if (parent.Status.Id == STATUS_WAITING_EVALUATION && Entity.Parent.Any(s => s.Email == parent.Email))
                errors.Add(new DbValidationError("Email", "Já existe um usuário cadastrado com este email."));

            if (parent.Students != null)
            {
                parent.Students.ForEach(s =>
                {
                    if (s.BirthDate == DateTime.MinValue || s.BirthDate.Date >= DateTime.Now.Date)
                        errors.Add(new DbValidationError("Birthdate", "Data nascimento inválida."));

                    if (string.IsNullOrEmpty(s.Name))
                        errors.Add(new DbValidationError("Student Name ", "Nome do estudante é obrigatório."));

                    if (s.Period == null)
                        errors.Add(new DbValidationError("Period", "Periodo é obrigatório."));
                });
            }

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
            Parent _parent = Entity.Parent.SingleOrDefault(s => s.Id == parent.Id);
            Entity.Phone.RemoveRange(_parent.Phone);
            Entity.Students.RemoveRange(_parent.Students);
            Entity.Parent.Remove(_parent);

            parent.Students.ForEach(student => 
            { 
                student.Period = Entity.Period.Single(x => x.Id == student.Period.Id); 
            });

            parent.Status = Entity.Status.Single(x => x.Id == parent.Status.Id);
            parent.ModifiedAt = DateTime.Now;
            Entity.Parent.Add(parent);            

            return Entity.SaveChanges() > 0 ? parent : null;
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

            if (parent.Status.Id == STATUS_INACTIVE)
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
