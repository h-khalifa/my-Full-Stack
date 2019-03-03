using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace People_CRUD.Models
{
    public class PersonRepo:IPersonRepo
    {
        private PeopleEntities Context;

        public PersonRepo()
        {
            this.Context = new PeopleEntities();
        }

        public bool AddPerson(person person)
        {
            if (!String.IsNullOrEmpty(person.Name))
            {
                Context.people.Add(person);
                Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeletePerson(int Id)
        {
            try
            {
                person p = new person() { Id = Id };
                Context.people.Attach(p);
                Context.people.Remove(p);
                Context.SaveChanges();
                return true;
            }
            catch (NullReferenceException e)
            {
                return false;
            }
        }

        public bool EditPerson(person person)
        {
            try
            {
                Context.people.Attach(person);
                Context.SaveChanges();
                return true;
            }
            catch (NullReferenceException e)
            {
                return false;
            }
        }

        public IEnumerable<person> GetAll()
        {
            return Context.people;
        }

        public IEnumerable<string> GetPeopleFullNames()
        {
            return Context.people.Select(p => p.Name);
        }

        public person GetPersonById(int Id)
        {
            //not null
            person person = Context.people.Find(Id);
            return person;
        }
    }
}