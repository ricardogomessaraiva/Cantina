using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Contexts;
using System.Collections.Generic;
using System.Linq;
using Services;

namespace UserTest
{
    [TestClass]
    public class UserValidationTest
    {
        private MealEntities entity;

        [TestInitialize()]
        public void Initialize()
        {
            entity = new MealEntities();
        }

        [TestMethod]
        public void UserShouldByValidateWhenAttributesAreValid()
        {                        
            var user = new Parent
            {
                Name = "Ricardo Gomes Saraiva",
                UserName = "ricardo.saraiva",
                Password = "12345",
                Email = "ricardogomessaraiva@hotmail.com",
                //Type = entity.Type.FirstOrDefault(s => s.Id == 3),
                Phone = new List<Phone> { new Phone { Number = 11986841296 } },
                Status = entity.Status.FirstOrDefault(s => s.Id == 1),
                Students = new List<Student>
                {
                    new Student
                    {
                        Name = "Gabriel Saraiva",
                        BirthDate = new DateTime(2006,7,23),
                        Period = entity.Period.FirstOrDefault(s => s.Id == 1)
                    }
                }
            };            

            var response = new UserService().Validate(user);
            Assert.AreEqual(response.Count, 0);                        
        }

        [TestMethod]
        public void UserShouldByNotValidateWhenAttributesFail()
        {
            var user = new Parent
            {
                Name = "",
                UserName = "",
                Password = "",
                Students = new List<Student>
                {
                    new Student
                    {
                        Name = "",
                        BirthDate = DateTime.MinValue, //When has no birthdate
                        Period = entity.Period.FirstOrDefault(s => s.Id == 1)
                    }
                },
                Email = "new_user@hotmail.com",
                //Type = entity.Type.FirstOrDefault(s => s.Id == 3),
                Phone = new List<Phone> { new Phone { Number = 11986841296 } },
                Status = entity.Status.FirstOrDefault(s => s.Id == 1)                
            };

            var response = new UserService().Validate(user);
            Assert.AreEqual(response.Count, 7);
        }

        [TestMethod]
        public void ShouldGiveErrorWhenEmailAlreadyIsRegistred()
        {
            var user = new Parent
            {
                Name = "Ricardo Gomes Saraiva",
                UserName = "ricardo.saraiva",
                Password = "12345",
                Email = "ricardogomessaraiva@hotmail.com",
                //Type = entity.Type.FirstOrDefault(s => s.Id == 3),
                Phone = new List<Phone> { new Phone { Number = 1136022451 } },
                Status = entity.Status.FirstOrDefault(s => s.Id == 1),
                Students = new List<Student>
                {
                    new Student
                    {
                        Name = "Gabriel Saraiva",
                        BirthDate = new DateTime(2006,7,23),
                        Period = entity.Period.FirstOrDefault(s => s.Id == 1)
                    }
                }
            };

            var response = new UserService().Validate(user);
            Assert.AreEqual(response.Count, 1);
        }

        [TestMethod]
        public void ShouldGiveErrorWhenEmailIsInvalid()
        {
            var user = new Parent
            {
                Name = "Ricardo Gomes Saraiva",
                UserName = "ricardo.saraiva",
                Password = "12345",
                Email = "ricardogomessaraiva@com",
                //Type = entity.Type.FirstOrDefault(s => s.Id == 3),
                Phone = new List<Phone> { new Phone { Number = 1136022451 } },
                Status = entity.Status.FirstOrDefault(s => s.Id == 1),
                Students = new List<Student>
                {
                    new Student
                    {
                        Name = "Gabriel Saraiva",
                        BirthDate = new DateTime(2006,7,23),
                        Period = entity.Period.FirstOrDefault(s => s.Id == 1)
                    }
                }
            };

            var response = new UserService().Validate(user);
            Assert.AreEqual(response.Count, 1);
        }

        [TestMethod]
        public void ShouldGiveErrorBirthDateEqualsOrGreaterThanToday()
        {
            var user = new Parent
            {
                Name = "Ricardo Gomes Saraiva",
                UserName = "ricardo.saraiva",
                Password = "12345",
                Email = "new_user@hotmail.com",
                //Type = entity.Type.FirstOrDefault(s => s.Id == 3),
                Phone = new List<Phone> { new Phone { Number = 1136022451 } },
                Status = entity.Status.FirstOrDefault(s => s.Id == 1),
                Students = new List<Student>
                {
                    new Student
                    {
                        Name = "Gabriel Saraiva",
                        BirthDate = DateTime.Now,
                        Period = entity.Period.FirstOrDefault(s => s.Id == 1)
                    }
                }
            };

            var response = new UserService().Validate(user);
            Assert.AreEqual(response.Count, 1);
        }

        [TestMethod]
        public void ShouldGiveErrorWhenHasNoPhoneNumber()
        {
            var user = new Parent
            {
                Name = "Ricardo Gomes Saraiva",
                UserName = "ricardo.saraiva",
                Password = "12345",
                Email = "new_user@hotmail.com",
                //Type = entity.Type.FirstOrDefault(s => s.Id == 3),
                Phone = new List<Phone> { new Phone { Number = 0 } },
                Status = entity.Status.FirstOrDefault(s => s.Id == 1),
                Students = new List<Student>
                {
                    new Student
                    {
                        Name = "Gabriel Saraiva",
                        BirthDate = new DateTime(2006,7,23),
                        Period = entity.Period.FirstOrDefault(s => s.Id == 1)
                    }
                }
            };

            var response = new UserService().Validate(user);
            Assert.AreEqual(response.Count, 1);
        }

        [TestMethod]
        public void ShouldGiveEqualsTrueWhenCompareHashedPassword()
        {
            var pass = "admin";
            var hashedPass = BCrypt.Net.BCrypt.HashPassword("admin");
            var result = BCrypt.Net.BCrypt.Verify(pass, hashedPass);            

            Assert.AreEqual(result, true);
        }

        [TestCleanup]
        public void EndOfTests()
        {
            entity.Dispose();
        }
    }
}
