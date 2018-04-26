using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Handin33.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace Handin33.Controllers
{
    public class PersonController : ApiController
    {
        private const string _endpointUrl = "https://localhost:8081";
        private const string _key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private readonly DocumentClient _client;

        private PersonRepository _repository;
        

        public PersonController()
        {
            _client = new DocumentClient(new Uri(_endpointUrl), _key);
            Setup().Wait(); //Wait for setup to complete

            _repository = new PersonRepository(_client, "PersonDB", "PersonCollection");
        }

        private async Task Setup()
        {
          
            _client.CreateDatabaseIfNotExistsAsync(new Database() { Id = "PersonDB" }).Wait(2000);
            _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("PersonDB"),
                new DocumentCollection { Id = "PersonCollection" }).Wait(2000);

        }
        // GET: api/Person
        public async Task<IEnumerable<Person>> Get()
        {
            return await _repository.ReadAll();
        }

        // GET: api/Person/5
        public async Task<Person> Get(string id)
        {
            return await _repository.Read(id);
        }

        // POST: api/Person
        public async Task Post([FromBody]Person p)
        {
            //Person p = await Task<Person>.Factory.StartNew(() => JsonConvert.DeserializeObject<Person>(value));
            await _repository.Create(p);

        }

        // PUT: api/Person/5
        public async Task Put(int id, [FromBody]Person p)
        {
            //Person p = await Task<Person>.Factory.StartNew(() => JsonConvert.DeserializeObject<Person>(value));

            if (p == null)
            {
                return;
            }
            await _repository.Update(p.Id, p);

        }

        // DELETE: api/Person/5
        public async Task Delete(string id)
        {

            if (id == null)
            {
                return;
            }

            await _repository.Delete(id);

        }
    }
}
