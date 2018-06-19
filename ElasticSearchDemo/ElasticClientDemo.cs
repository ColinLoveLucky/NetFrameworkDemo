using Elasticsearch.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchDemo
{
    public class ElasticClientDemo
    {
        public static ElasticLowLevelClient GetElaticClient()
        {
            var settings = new ConnectionConfiguration(new Uri("http://172.16.36.130:9200")).
                RequestTimeout(TimeSpan.FromMinutes(2));
            var lowLevelClient = new ElasticLowLevelClient(settings);
            return lowLevelClient;
        }
        public static ElasticLowLevelClient GetElasticClientFromPools()
        {
            var uris = new[]
            {
                new Uri("http://172.16.36.130:9200"),
                new Uri("http://172.16.36.62:9201/"),
            };
            var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionConfiguration(connectionPool);
            var lowLevelClient = new ElasticLowLevelClient(settings);
            return lowLevelClient;
        }
        public static ElasticLowLevelClient GetElasticClientByCompleted()
        {
            var settings = new ConnectionConfiguration(new Uri("http://172.16.36.130:9200")).
                OnRequestCompleted(apoCallDetails =>
                {
                    if (apoCallDetails.HttpStatusCode == 418)
                        throw new TimeoutException();
                });
            var lowLevelClinet = new ElasticLowLevelClient(settings);
            return lowLevelClinet;
        }
        public static void CreatePeopleIndex()
        {
            var person = new Person()
            {
                FirstName = "Martijn",
                LastName = "Laarman"
            };
            var client = GetElaticClient();
            var indexResponse = client.Index<byte[]>("people", "person", "1", person);

            byte[] responseBytes = indexResponse.Body;
        }
        public static async Task<int> CreatePeopleAsyncIndex()
        {
            var person = new Person()
            {
                FirstName = "Zhang",
                LastName = "Lucky"
            };
            var client = GetElaticClient();
            var asyncIndexResponse = await client.IndexAsync<string>("people", "person", "2", person);
            string responseString = asyncIndexResponse.Body;
            return (int)HttpStatusCode.OK;
        }
        public static void CreateIndexBulk()
        {
            var people = new object[]
            {
                new { index = new { _index = "people", _type = "person", _id = "1"  }},
                new { FirstName = "Hello", LastName = "Kitte" },
                new { index = new { _index = "people", _type = "person", _id = "2"  }},
                new { FirstName = "Greg", LastName = "Marzouka" },
                new { index = new { _index = "people", _type = "person", _id = "3"  }},
                new { FirstName = "Russ", LastName = "Cam" },
            };
            var client = GetElasticClientFromPools();
            var indexResponse = client.Bulk<Stream>(people);
            Stream response = indexResponse.Body;
        }
        public static void ResponseString()
        {
            var client = GetElaticClient();
            var searchResponse = client.Search<string>("people", "person", new
            {
                from = 0,
                size = 10,
                query = new
                {
                    match = new
                    {
                        field = "firstName",
                        query = "Martijn"
                    }
                }
            });
            var successful = searchResponse.Success;
            var responseJson = searchResponse.Body;
        }
        public static void ResponseByte()
        {
            var client = GetElasticClientFromPools();
            var searchResponse = client.Search<byte[]>("people", "person", @"{
                    ""from"":0,
                    ""size"":10,
                    ""query"":{
                        ""match"":{
                            ""field"":""FirstName"",
                            ""query"":""Zhang""
                    }
                }");
            var responseBytes = searchResponse.Body;
        }
        public static void HandingErrors()
        {
            var client = GetElasticClientFromPools();
            var serachResponse = client.Search<byte[]>("people", "person", new
            {
                match_all = new { }
            }
                );
            var success = serachResponse.Success;
            var successOrKnownError = serachResponse.SuccessOrKnownError;
            var serverError = serachResponse.ServerError;
            var exception = serachResponse.OriginalException;
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
