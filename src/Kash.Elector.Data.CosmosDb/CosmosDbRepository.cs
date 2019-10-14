using Kash.Elector.Data.CosmosDb.SeedWork;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kash.Elector.Data.CosmosDb
{
    public class CosmosDbRepository<TItem> : IRepository<TItem> where TItem : class, IEntity, IOrganizativeUnit
    {
        /// <summary>
        /// Datos de configuración del repositorio
        /// </summary>
        private readonly CosmosDbConfiguration CosmosDbConfig;

        /// <summary>
        /// Enlace a la colección de cosmos
        /// </summary>
        private Uri CollectionUri;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cosmosDbConfig"></param>
        public CosmosDbRepository(CosmosDbConfiguration cosmosDbConfig)
        {
            CosmosDbConfig = cosmosDbConfig ?? throw new ArgumentNullException(nameof(cosmosDbConfig));

            CollectionUri = UriFactory.CreateDocumentCollectionUri(cosmosDbConfig.DatabaseId, cosmosDbConfig.CollectionId);
        }

        /// <summary>
        /// Devuelve el cliente para ejecutar operaciones sobre la base de datos
        /// </summary>
        /// <returns></returns>
        private DocumentClient CreateClient()
        {
            return new DocumentClient(
                new Uri(CosmosDbConfig.ServiceEndpoint),
                CosmosDbConfig.AuthKeyOrResourceToken,
                new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver() }
                );
        }

        //protected string GetDocumentId(object[] keyValues)
        //{
        //    return string.Join(",", keyValues.Select(k => k.ToString()));
        //}

        /// <summary>
        /// Crea un document link
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected Uri GetDocumentUri(string id)
        {
            string documentId = id;
            return UriFactory.CreateDocumentUri(CosmosDbConfig.DatabaseId, CosmosDbConfig.CollectionId, documentId);
        }

        /// <inheritdoc />
        public async Task<TItem> Add(TItem item)
        {
            if (string.IsNullOrEmpty(item.Id))
            {
                item.Id = Guid.NewGuid().ToString();
            }

            using (var client = CreateClient())
            {
                var response = await client.CreateDocumentAsync(CollectionUri, item, new RequestOptions { });
            }

            return item;
        }

        /// <inheritdoc />
        public async Task<TItem> Find(Expression<Func<TItem, bool>> predicate)
        {
            using (var client = CreateClient())
            {
                var query = client.CreateDocumentQuery<TItem>(CollectionUri, new FeedOptions() { MaxItemCount = 1 })
                    .Where(predicate).Select(d => d);

                var docs = await ((IDocumentQuery<TItem>)query).ExecuteNextAsync<TItem>();

                foreach (var item in docs)
                {
                    return item;
                }
                return null;
            }
        }

        /// <inheritdoc />
        public async Task Remove(TItem item)
        {
            var documentUri = GetDocumentUri(item.Id);
            using (var client = CreateClient())
            {
                var response = await client.DeleteDocumentAsync(
                    documentUri,
                    new RequestOptions() { PartitionKey = new PartitionKey(item.OrganizativeUnitId.ToString()) });
            }
        }

        /// <inheritdoc />
        public async Task Remove(string id)
        {
            var documentUri = GetDocumentUri(id);
            using (var client = CreateClient())
            {
                var response = await client.DeleteDocumentAsync(
                    CollectionUri,
                    new RequestOptions() { PartitionKey = new PartitionKey(Undefined.Value) });
            }
        }

        /// <inheritdoc />
        public async Task Remove(Expression<Func<TItem, bool>> predicate)
        {
            using (var client = CreateClient())
            {
                var query = client.CreateDocumentQuery<TItem>(CollectionUri, new FeedOptions() { MaxItemCount = 1 });
                query.Where(predicate).Select(d => d.Id);

                var docIds = await ((IDocumentQuery<TItem>)query).ExecuteNextAsync<string>();

                var id = docIds.FirstOrDefault();
                await Remove(id);
            }
        }

        /// <inheritdoc />
        public async Task<TItem> Update(TItem item)
        {
            var documentUri = GetDocumentUri(item.Id);
            using (var client = CreateClient())
            {
                var requestOptions = new RequestOptions
                {
                    AccessCondition = new AccessCondition
                    {
                        Condition = item._etag,
                        Type = AccessConditionType.IfMatch
                    }
                };
                var response = await client.ReplaceDocumentAsync(documentUri, item, requestOptions);
            }

            return item;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TItem>> Get(
            Expression<Func<TItem, bool>> predicate,
            OrderedQueryableExpression<TItem>[] orderExpressions = null)
        {
            using (var client = CreateClient())
            {
                // Filtro
                var filter = client.CreateDocumentQuery<TItem>(CollectionUri).Where(predicate);

                // Ordenar
                filter = ApplySort(filter, orderExpressions);

                filter = filter.Select(d => d);
                var query = filter.AsDocumentQuery();

                var result = await query.ExecuteNextAsync<TItem>();

                return result;
            }
        }

        /// <inheritdoc />
        public async Task<KeyValuePair<string, IEnumerable<TItem>>> Get(
            Expression<Func<TItem, bool>> predicate,
            int pageSize,
            string continuationToken,
            OrderedQueryableExpression<TItem>[] orderExpressions = null)
        {
            var feedOptions = new FeedOptions
            {
                MaxItemCount = pageSize,
                EnableCrossPartitionQuery = true,
                // IMPORTANT: Set the continuation token (NULL for the first ever request/page)
                RequestContinuation = !string.IsNullOrWhiteSpace(continuationToken) ? continuationToken : null
            };

            using (var client = CreateClient())
            {
                // Filtro
                var filter = client.CreateDocumentQuery<TItem>(CollectionUri, feedOptions).Where(predicate);

                // Ordenar
                filter = ApplySort(filter, orderExpressions);

                filter = filter.Select(d => d);
                var query = filter.AsDocumentQuery();
                FeedResponse<TItem> feedResponse = await query.ExecuteNextAsync<TItem>();

                var items = new List<TItem>();
                foreach (var t in feedResponse)
                    items.Add(t);

                var result = new KeyValuePair<string, IEnumerable<TItem>>(feedResponse.ResponseContinuation, items);

                return result;
            }
        }

        /// <inheritdoc />
        public async Task<int> Count(Expression<Func<TItem, bool>> predicate)
        {
            using (var client = CreateClient())
            {
                var count = await client.CreateDocumentQuery<TItem>(CollectionUri).Where(predicate).CountAsync();

                return count;
            }
        }

        private IQueryable<TItem> ApplySort(IQueryable<TItem> query, OrderedQueryableExpression<TItem>[] expressions)
        {
            if (expressions == null || !expressions.Any())
                return query;

            var sorting = (expressions.First().orderDescending) ? query.OrderByDescending(expressions.First().Predicate) : query.OrderBy(expressions.First().Predicate);

            for (int i = 1; i < expressions.Count(); i++)
            {
                sorting = (expressions[i].orderDescending) ? sorting.ThenByDescending(expressions[i].Predicate) : sorting.ThenBy(expressions[i].Predicate);
            }
            query = sorting;

            return query;
        }
    }
}
