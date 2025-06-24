using PersonApi.Data;
using PersonApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace TestPerson
{
    public class Seeding
    {
        public static void InitializeTestDB(PersonAPIContext db) {

            db.PersonTypes.AddRange(GetPersonTypes());
            db.SaveChanges();

            db.Persons.AddRange(GetPersons());
            db.SaveChanges();
        }

        private static List<PersonType> GetPersonTypes()
        {
            return new List<PersonType>
            {
                new PersonType { Id = 3, Description = "Analyst" },
                new PersonType { Id = 4, Description = "Director" }
            };
        }

        private static List<Person> GetPersons()
        {
            return new List<Person>() {
                new Person() { Id = 3 , Age = 27, Name = "Animesh", PersonTypeId = 1},
                new Person() { Id = 4 , Age = 30, Name = "Himel", PersonTypeId = 2},
            };
        }
    }
}
