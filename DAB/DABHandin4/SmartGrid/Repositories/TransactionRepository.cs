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

        private readonly string _databaseName;
        private readonly string _collectionName;
        private readonly DocumentClient _client;

        public TransactionRepository(DocumentClient client, string databaseName, string collectionName)
        {
            this._client = client;
            this._databaseName = databaseName;
            this._collectionName = collectionName;
        }
        public async Task Create(Transaction Transaction)
        {
            try
            {
                await this._client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_databaseName, _collectionName, Transaction.Id));
                Console.WriteLine("Found {0}", Transaction.Id);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this._client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName),
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
            return await _client.ReadDocumentAsync<Transaction>(UriFactory.CreateDocumentUri(_databaseName, _collectionName, documentName));
        }

        public async Task<IEnumerable<Transaction>> ReadAll()
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Transaction> TransactionQuery = this._client.CreateDocumentQuery<Transaction>(
                UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName),
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

            await this._client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseName, _collectionName, documentName), updatedTransaction);
            Console.WriteLine("Replaced Transaction {0}", documentName);


        }


        public async Task Delete(string documentName)
        {
            await this._client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_databaseName, _collectionName, documentName));
            Console.WriteLine("Deleted Transaction {0}", documentName);

        }
    }
}