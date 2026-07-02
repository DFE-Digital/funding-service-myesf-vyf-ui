using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// CosmosDb service.
    /// </summary>
    /// <typeparam name="T">The generic type.</typeparam>
    public class CosmosDbService<T> : ICosmosDbService<T>
        where T : CosmosDocument
    {
        private readonly ILoggerAdapter<CosmosDbService<T>> _logger;
        private readonly Container _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDbService{T}"/> class.
        /// </summary>
        /// <param name="cosmosClient">The cosmos client.</param>
        /// <param name="cosmosDbConfiguration">The cosmos Db Configuration.</param>
        /// <param name="logger">The logger service.</param>
        public CosmosDbService(
            CosmosClient cosmosClient,
            IOptions<CosmosDbConfiguration> cosmosDbConfiguration,
            ILoggerAdapter<CosmosDbService<T>> logger)
        {
            var instance = (T)Activator.CreateInstance(typeof(T));
            _logger = logger;
            _container = cosmosClient.GetContainer(
                cosmosDbConfiguration.Value.DatabaseName,
                instance.CollectionName);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TModel>> RunQueryAsync<TModel>(string query)
        {
            var feed = _container.GetItemQueryIterator<TModel>(query);
            var feedResponse = await feed.ReadNextAsync();

            var results = new List<TModel>();
            results.AddRange(feedResponse.ToList());

            return results;
        }

        /// <inheritdoc />
        public async Task<string> RunQueryAllAsync<Tdynamic>(string query)
        {
            var feed = _container.GetItemQueryIterator<Tdynamic>(query);
            var feedResponse = await feed.ReadNextAsync();

            var serialised = JsonConvert.SerializeObject(feedResponse);


            return serialised;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetAllAsync(List<Expression<Func<T, bool>>> filters = null)
        {
            IQueryable<T> query = _container.GetItemLinqQueryable<T>();

            if (filters?.Any() == true)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            var results = new List<T>();

            var feed = query.ToFeedIterator();

            while (feed.HasMoreResults)
            {
                var feedResponse = await feed.ReadNextAsync();
                results.AddRange(feedResponse.ToList());
            }

            var resultsData = string.Join(',', results.Select(GetItemData));
            _logger?.LogInformation($"Result => {resultsData} for {nameof(GetAllAsync)}");

            return results;
        }

        /// <inheritdoc />
        public async Task<T> GetAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
                _logger?.LogInformation(Message(null, GetItemData(response), nameof(AddAsync), id));

                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger?.LogInformation(ex, null);
                return default;
            }
        }

        /// <inheritdoc />
        public async Task<bool> AddAsync(T item)
        {
            var response = await _container.CreateItemAsync(item, new PartitionKey(item.Id));
            _logger?.LogInformation(Message(item, GetItemData(response), nameof(AddAsync)));

            return response.StatusCode == HttpStatusCode.Created;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(string id, T item)
        {
            var response = await _container.UpsertItemAsync(item, new PartitionKey(id));
            _logger?.LogInformation(Message(item, GetItemData(response), nameof(UpdateAsync)));

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(string id, T item)
        {
            try
            {
                var response = await _container.UpsertItemAsync<T>(item, new PartitionKey(id));
                _logger?.LogInformation(Message(null, GetItemData(response), nameof(DeleteAsync), id));

                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception exception)
            {
                _logger?.LogInformation(exception, null);
                return false;
            }
        }

        /// <inheritdoc />
        public async Task<int> GetCountAsync(List<Expression<Func<T, bool>>> filters = null)
        {
            IQueryable<T> query = _container.GetItemLinqQueryable<T>();

            if (filters?.Any() == true)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            int count = await query.CountAsync();
            _logger?.LogInformation($"Result => {count} for {nameof(GetCountAsync)}");
            return count;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetPagination(int skip, int take, List<Expression<Func<T, bool>>> filters = null)
        {
            var key = string.Empty;

            var query = _container.GetItemLinqQueryable<T>(false);

            if (filters?.Any() == true)
            {
                foreach (var filter in filters)
                {
                    query = (IOrderedQueryable<T>)query.Where(filter);
                }
            }

            var linqQuery = query.Skip(skip)
                         .Take(take)
                         .ToQueryDefinition();

            var results = await _container.GetItemQueryIterator<T>(linqQuery).ReadNextAsync();

            var resultsData = results.ToList();
            _logger?.LogInformation($"Result => {resultsData} for {nameof(GetPagination)}");

            return resultsData;
        }

        private static string GetItemData(T item)
        {
            if (item == null)
            {
                return "null";
            }

            return JsonConvert.SerializeObject(
                item,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        private static string Message(T item, string actionResult, string actionName, string id = "")
        {
            if (item != null)
            {
                return $"CosmosDb action {actionName} for {GetItemData(item)} had result {actionResult}";
            }

            return $"CosmosDb action {actionName} for {id} had result {actionResult}";
        }
    }
}