using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using SmartGrid.Models;

namespace SmartGrid.Repositories
{
    public class TransactionRepository
    {

        private string databaseName;
        private string collectionName;
        private DocumentClient client;

        private List<Task> _transactionsToExecute;
        public TransactionRepository(DocumentClient client, string databaseName, string collectionName)
        {
            this.client = client;
            this.databaseName = databaseName;
            this.collectionName = collectionName;
            _transactionsToExecute = new List<Task>();
        }
        public async Task Create(Transaction Transaction)
        {
            try
            {
                await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, Transaction.Id));
                Console.WriteLine("Found {0}", Transaction.Id);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                        Transaction);
                    Console.WriteLine("Created Transaction {0}", Transaction.Id);
                }
                else
                {
                    throw;
                }
            }


        }

        public async Task<Transaction> Read(string documentName)
        {
            return await client.ReadDocumentAsync<Transaction>(UriFactory.CreateDocumentUri(databaseName, collectionName, documentName));
        }

        public async Task<IEnumerable<Transaction>> ReadAll()
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Transaction> TransactionQuery = this.client.CreateDocumentQuery<Transaction>(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                "SELECT * FROM c",
                queryOptions);

            List<Transaction> data = new List<Transaction>();
            foreach (Transaction p in TransactionQuery)
            {
                data.Add(p);
            }

            return data;
        }
        public async Task Update(string documentName, Transaction updatedTransaction)
        {

            await this.client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, documentName), updatedTransaction);
            Console.WriteLine("Replaced Transaction {0}", documentName);


        }


        public async Task Delete(string documentName)
        {
            await this.client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, documentName));
            Console.WriteLine("Deleted Transaction {0}", documentName);

        }

        public void Commit()
        {
            foreach (var transaction in _transactionsToExecute.ToArray())
            {
                Task.WaitAny(transaction);
                _transactionsToExecute.Remove(transaction);


            }
        }

        public async Task PrintTransaction(string documentName)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Now execute the same query via direct SQL
            IQueryable<Transaction> TransactionQuery = this.client.CreateDocumentQuery<Transaction>(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                "SELECT * FROM Transaction where Transaction.Id = " + string.Format("\'{0}\'", documentName),
                queryOptions);

            Console.WriteLine("Getting Transaction with ID {0}", documentName);
            foreach (Transaction transaction in TransactionQuery)
            {
                Console.WriteLine("Found Transaction: {0} with between {1} and {2} (seller - buyer)", transaction.Id, transaction.Producer, transaction.Consumer);
                Console.WriteLine("Total kWh transferred: {0} with price per kWh: {1}", transaction.KwhAmount, transaction.PricePerKwh);
                Console.WriteLine("Total price: {0}", transaction.TotalPrice);
                Console.WriteLine();
                
            }
        }

        public async Task PrintAllTransactions()
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Now execute the same query via direct SQL
            IQueryable<Transaction> TransactionQuery = this.client.CreateDocumentQuery<Transaction>(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                "SELECT * FROM Transaction",
                queryOptions);

            Console.WriteLine("Running direct SQL query...");
            foreach (Transaction transaction in TransactionQuery)
            {
                Console.WriteLine("Found Transaction: {0} with between {1} and {2} (seller - buyer)", transaction.Id, transaction.Producer, transaction.Consumer);
                Console.WriteLine("Total kWh transferred: {0} with price per kWh: {1}", transaction.KwhAmount, transaction.PricePerKwh);
                Console.WriteLine("Total price: {0}", transaction.TotalPrice);
                Console.WriteLine();

            }
        }
    }
}