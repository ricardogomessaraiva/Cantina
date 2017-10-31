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
        private const int STATUS_EVERYONE = 0; //Don't exists at database, just to ignore in a filter
        private const int STATUS_ACTIVE = 1;
        private const int STATUS_INACTIVE = 2;
        private const int STATUS_WAITING_EVALUATION = 3;
        private const int STATUS_BLOCKED = 4;

        readonly MealEntities Entity = new MealEntities();

        public List<DbValidationError> Validate(Parent parent)
        {
            if (parent.Students.Count == 0)
                parent.Students = null;

            Entity.Parent.Add(parent);
            var errors = new List<DbValidationError>();

            if (Entity.GetValidationErrors().Count() > 0)
                errors.AddRange(Entity.GetValidationErrors().First().ValidationErrors);

            if (parent.Email != null && parent.Status.Id == STATUS_WAITING_EVALUATION)
            {
                var isValid = Regex.IsMatch(parent.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (!isValid)
                    errors.Add(new DbValidationError("Email", "Digite um e-mail válido."));
            }

            if (parent.Status.Id == STATUS_WAITING_EVALUATION && parent.Id == 0 && Entity.Parent.Any(s => s.Email == parent.Email))
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
            var entity = new MealEntities();
            parent.Status = entity.Status.FirstOrDefault(s => s.Id == STATUS_WAITING_EVALUATION);
            //Set user period by period id
            parent.Students.ForEach(s =>
            {
                s.Period = entity.Period.First(p => p.Id == s.Period.Id);
            });

            var model = entity.Parent.Add(parent);
            entity.SaveChanges();
            return model;
        }

        public Parent Update(Parent parent)
        {
            var entity = new MealEntities();

            Parent _parent = entity.Parent.SingleOrDefault(s => s.Id == parent.Id);
            entity.Phone.RemoveRange(_parent.Phone);
            entity.Students.RemoveRange(_parent.Students);
            entity.Parent.Remove(_parent);

            parent.Students.ForEach(student =>
            {
                student.Period = Entity.Period.Find(student.Period.Id);
            });

            parent.Status = entity.Status.Find(parent.Status.Id);
            parent.Status = entity.Status.FirstOrDefault(x => x.Id == parent.Status.Id);
            parent.ModifiedAt = DateTime.Now;
            entity.Parent.Add(parent);

            return entity.SaveChanges() > 0 ? parent : null;
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
            if (parent.Status != null && parent.Status.Id == STATUS_EVERYONE)
            {
                return Entity.Parent
                            .Where(x => x.Name.Contains((parent.Name != null ? parent.Name : x.Name)) &&
                                        x.UserName.Contains((parent.UserName != null ? parent.UserName : x.UserName)) &&
                                        x.Email.Contains((parent.Email != null ? parent.Email : x.Email)))
                            .OrderBy(x => x.CreatedAt)
                            .ToList();
            }

            if (parent.Status != null)
            {
                return Entity.Parent
                    .Where(x => x.Name.Contains((parent.Name != null ? parent.Name : x.Name)) &&
                                x.UserName.Contains((parent.UserName != null ? parent.UserName : x.UserName)) &&
                                x.Email.Contains((parent.Email != null ? parent.Email : x.Email)) &&
                                x.Status.Id == parent.Status.Id)
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
