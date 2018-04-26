using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Handin33.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Handin33
{
    public class PersonRepository
    {
        private string databaseName;
        private string collectionName;
        private DocumentClient client;
        private List<Task> _transactionsToExecute;
        public PersonRepository(DocumentClient client, string databaseName, string collectionName)
        {
            this.client = client;
            this.databaseName = databaseName;
            this.collectionName = collectionName;
            _transactionsToExecute = new List<Task>();
        }




        public async Task Create(Person person)
        {
                try
                {
                    await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, person.Id));
                    Console.WriteLine("Found {0}", person.Id);
                }
                catch (DocumentClientException de)
                {
                    if (de.StatusCode == HttpStatusCode.NotFound)
                    {
                        await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                            person);
                        Console.WriteLine("Created Person {0}", person.Id);
                    }
                    else
                    {
                        throw;
                    }
                }


        }

        public async Task<Person> Read(string documentName)
        {
            return await client.ReadDocumentAsync<Person>(UriFactory.CreateDocumentUri(databaseName, collectionName, documentName));
        }

        public async Task<IEnumerable<Person>> ReadAll()
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable < Person > personQuery = this.client.CreateDocumentQuery<Person>(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                "SELECT * FROM c",
                queryOptions);

            List<Person> data = new List<Person>();
            foreach (Person p in personQuery)
            {
                data.Add(p);
            }

            return data;
        }
        public async Task Update(string documentName, Person updatedPerson)
        {

                await this.client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, documentName), updatedPerson);
                Console.WriteLine("Replaced Person {0}", documentName);


        }
        

        public async Task Delete(string documentName)
        {
                await this.client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, documentName));
                Console.WriteLine("Deleted Person {0}", documentName);

        }
        
        public void Commit()
        {
            foreach (var transaction in _transactionsToExecute.ToArray())
            {
                Task.WaitAny(transaction);
                _transactionsToExecute.Remove(transaction);


            }
        }

        public async Task PrintPerson(string documentName)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Now execute the same query via direct SQL
            IQueryable<Person> personQuery = this.client.CreateDocumentQuery<Person>(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                "SELECT * FROM Person where Person.Id = " + string.Format("\'{0}\'", documentName),
                queryOptions);

            Console.WriteLine("Getting person with ID {0}", documentName);
            foreach (Person person in personQuery)
            {
                Console.WriteLine("Found person: {0} with addresses:", person.FirstName);
                foreach (Email mail in person.Emails)
                    Console.WriteLine(mail.MailAddress);
                foreach (PublicAddress address in person.PublicAddresses)
                    Console.WriteLine(address.StreetName + " " + address.HouseNumber);
                foreach (PhoneNumber number in person.PhoneNumbers)
                    Console.WriteLine(number.Number);
                Console.WriteLine();
            }
        }

        public async Task PrintAllPersons()
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Now execute the same query via direct SQL
            IQueryable<Person> personQuery = this.client.CreateDocumentQuery<Person>(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                "SELECT * FROM Person",
                queryOptions);

            Console.WriteLine("Running direct SQL query...");
            foreach (Person person in personQuery)
            {
                Console.WriteLine("Found person: {0} with addresses:", person.FirstName);
                foreach (Email mail in person.Emails)
                    Console.WriteLine(mail.MailAddress);
                foreach (PublicAddress address in person.PublicAddresses)
                    Console.WriteLine(address.StreetName + " " + address.HouseNumber);
                foreach (PhoneNumber number in person.PhoneNumbers)
                    Console.WriteLine(number.Number);

                Console.WriteLine();
            }
        }




    }
}