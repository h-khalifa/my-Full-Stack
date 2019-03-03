using People_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;

namespace People_CRUD.Controllers
{
    public class PeopleController : ApiController
    {
        PersonRepo repo = new PersonRepo();

        [HttpGet]
        public IEnumerable<string> GetNames() {
            var names = repo.GetPeopleFullNames();
           return names;
        }
    }
}
