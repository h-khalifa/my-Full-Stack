using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People_CRUD.Models
{
    interface IPersonRepo
    {
        IEnumerable<person> GetAll();
        person GetPersonById(int Id);
        bool DeletePerson(int Id);
        bool EditPerson(person person);
        bool AddPerson(person person);
        IEnumerable<string> GetPeopleFullNames();
    }

}
