using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using SmartGrid.Models;
using SmartGrid.Repositories;

namespace SmartGrid.Controllers
{
    public class TransactionController : ApiController
    {
        private const string _endpointUrl = "https://localhost:8081";
        private const string _key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private readonly DocumentClient _client;

        private TransactionRepository _repository;


        public TransactionController()
        {
            _client = new DocumentClient(new Uri(_endpointUrl), _key);
            Setup().Wait(); //Wait for setup to complete

            _repository = new TransactionRepository(_client, "TransactionDB", "TransactionCollection");
        }

        private async Task Setup()
        {

            _client.CreateDatabaseIfNotExistsAsync(new Database() { Id = "TransactionDB" }).Wait(2000);
            _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("TransactionDB"),
                new DocumentCollection { Id = "TransactionCollection" }).Wait(2000);

        }
        // GET: api/Person
        public async Task<IEnumerable<Transaction>> Get()
        {
            return await _repository.ReadAll();
        }

        // GET: api/Transaction/5
        public async Task<Transaction> Get(string id)
        {
            return await _repository.Read(id);
        }

        // POST: api/Transaction
        public async Task Post([FromBody]Transaction p)
        {
            //Person p = await Task<Person>.Factory.StartNew(() => JsonConvert.DeserializeObject<Person>(value));
            await _repository.Create(p);

        }
        // POST: api/Transaction
        public async Task Post([FromBody]List<Transaction> p)
        {
            //Person p = await Task<Person>.Factory.StartNew(() => JsonConvert.DeserializeObject<Person>(value));
            foreach (Transaction t in p)
            {
                await _repository.Create(t);
            }
            

        }
    }
}
