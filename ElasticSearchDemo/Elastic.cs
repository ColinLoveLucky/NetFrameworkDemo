using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ElasticSearchDemo
{
    /// <summary>
    /// cluster.name: my-application
    //#
    //# ------------------------------------ Node ------------------------------------
    //#
    //# Use a descriptive name for the node:
    //#
    //node.name: "Colin"
    //#node.box_type: M
    //node.master: true
    //node.data: true
    //network.host: 172.16.36.130
    //http.port: 9200
    //discovery.zen.ping.unicast.hosts: ["172.16.36.130"]
    //http.cors.enabled: true
    //http.cors.allow-origin: "*"
    /// </summary>
    public class Elastic
    {
        public static ElasticClient GetElasticClient(string indexName)
        {
            var connectString = "http://127.0.0.1:9200";
            var nodeStr = connectString.Split('|');
            var nodes = nodeStr.Select(s => new Uri(s)).ToList();
            var connectionPool = new SniffingConnectionPool(nodes);
            var settings = new ConnectionSettings(connectionPool).DefaultIndex(indexName);
            var client = new ElasticClient(settings);
            return client;
        }

        public static void CreateIndex(string indexName)
        {
            var client = GetElasticClient("");
            //client.CreateIndex(r => r.Index(indexName).NumberOfReplicas(1).NumberOfShards(20));
            //client.Map<Order>(m => m.MapFromAttributes());
        }

        public static void WriteIndex()
        {
            var client = GetElasticClient("order");
            long i = GetMax();
            while (true)
            {
                i++;
                SetMax(i);
                Order order = new Order()
                {
                    Id = i,
                    FirstName = "我爱天安门",
                    LastName = "外派",
                    TotalPrice = Convert.ToDouble(i),
                    Content = "天使3412341234" + i.ToString(),
                    CreateTime = DateTime.Now
                };
                var addResult = client.Index(order);
                System.Console.WriteLine("第" + i + "行记录" + addResult.Created);

            }
        }

        public static long GetMax()
        {
            long reslt = 0;
            try
            {
                string k = System.IO.File.ReadAllText("1.txt");
                reslt = Convert.ToInt64(k);
            }
            catch (Exception)
            {

            }
            return reslt;
        }

        public static long SetMax(long max)
        {
            long reslt = 0;
            try
            {
                System.IO.File.WriteAllText("1.txt", max.ToString());
            }
            catch (Exception)
            {

            }
            return reslt;
        }

        public static void Search()
        {
            //  #region
            //  var client = GetElasticClient("order");
            //  QueryContainer termQuery = new TermQuery() { Field = "lastname", Value = "求和" };
            //  var result = client.Search<Order>(s => s
            //                  .Aggregations(a => a
            //                      .Sum("my_sum_agg", sa => sa
            //                          .Field(p => p.TotalPrice)
            //                      )
            //                  )
            //              );
            //  var agg = result.Aggs.Sum("my_sum_agg");


            //  var searchResults = client.Search<Order>(s => s
            //                  //  .Query(termQuery)  //带筛选条件
            //                  .Aggregations(r => r.Terms("firstname", r1 => r1.Field(r2 => r2.Lastname)
            //                      .OrderAscending("sumprice")
            //                      .Aggregations(y => y.Sum("sumprice", y1 => y1.Field(y2 => y2.TotalPrice))))));

            //  var carTypes = searchResults.Aggs.Terms("firstname");
            //  List<double> re = new List<double>();
            //  foreach (var carType in carTypes.Items)
            //  {
            //      string key = carType.Key;
            //      System.Console.WriteLine("key:" + key + " total:" + carType.Sum("sumprice").Value);
            //      re.Add((double)carType.Sum("sumprice").Value);
            //  }
            //  //List<SumTotalPrice> orders = searchResults.Documents.ToList();
            //  //   System.Console.WriteLine(orders.Count() + " total:" + searchResults.Total);
            //  //System.Console.WriteLine(searchResults.RequestInformation);
            //  System.Console.ReadLine();

            //  #endregion


            //  #region 多条件精确匹配
            //  var client = GetElasticClient("order");
            //  QueryContainer termQuery = new TermQuery() { Field = "firstname", Value = "测试" };
            //  QueryContainer wholeWordQuery = new TermQuery() { Field = "lastname", Value = "addddd" };
            //  //非索引字段。无法查询 此查询条件无效
            //  QueryContainer termQuery1 = new TermQuery() { Field = "content", Value = "  天空是百度的1696" };
            //  termQuery = (termQuery || wholeWordQuery) || termQuery1;

            //  var searchResults = client.Search<Order>(s => s
            //      .From(0)
            //      .Size(10)
            //      .Query(termQuery)
            //      );
            //  List<Order> orders = searchResults.Documents.ToList();
            //  System.Console.WriteLine(orders.Count() + " total:" + searchResults.Total);
            //  System.Console.ReadLine();

            //  #endregion

            //  #region 精确匹配
            //  var client = ElasticsearchHelper.GetElasticClient("order");
            //  var searchResults = client.Search<Order>(s => s
            //    .From(0)
            //    .Size(10)
            //    .Query(q => q.Term(r => r.OnField(k => k.Firstname).Value("棉花")))
            //    .Highlight(h => h.OnFields(e => e.OnField("firstname")
            //                      .PreTags("<b style='color:black'>")
            //                      .PostTags("</b>")))
            //    .Sort(r => r.Descending().OnField(q => q.Createtime))
            //    );
            //  List<Order> orders = searchResults.Documents.ToList();

            //  System.Console.WriteLine(orders.Count() + " total:" + searchResults.Total);
            //  System.Console.ReadLine();

            //  #endregion

            //  //  //查询索引字段 带 裤的  类似 数据库的  like '裤%'
            //  var client = ElasticsearchHelper.GetElasticClient("order");
            //  var searchResults = client.Search<Order>(s => s
            //.From(0)
            //.Size(10)
            //.QueryString("裤*")
            //);
            //  System.Console.WriteLine(" total:" + searchResults.Total);
            //  System.Console.ReadLine();


            //  #region ID查询

            //  var searchResults = client.Search<Order>(s => s
            //     .From(0)
            //     .Size(10)
            //     .Query(q => q.Ids(r => r.Values(19)))
            //     );

            //  #endregion

            //  #region 时间范围查询
            //  var client = ElasticsearchHelper.GetElasticClient("order");
            //  //  QueryContainer termQuery = new TermQuery() { Field = "firstname", Value = "测试" };
            //  var format = "yyyy-MM-dd'T'HH:mm:ss.fff+08:00";
            //  var lowerBound = DateTime.Now.AddMinutes(-20);

            //  var searchResults = client.Search<Order>(s => s
            //      // .Query(r => termQuery && r.Range(st => st.OnField(st1 => st1.Createtime).GreaterOrEquals(lowerBound, format)))
            //      .Query(r => r.Range(st => st.OnField(st1 => st1.Createtime).GreaterOrEquals(lowerBound, format)))
            //      .SortDescending(r => r.Createtime)
            //      .From(0)
            //      .Size(10)
            //      );
            //  List<Order> orders = searchResults.Documents.ToList();
            //  var k = searchResults.RequestInformation;
            //  System.Console.WriteLine(orders.Count() + " total:" + searchResults.Total);
            //  System.Console.ReadLine();

            //  #endregion
        }

        public static void DeleteRowById(long id)
        {
            var client = GetElasticClient("order");

            //client.Delete<Order>(r => r.Id(id));
        }
    }

    [ElasticsearchType(IdProperty = "id", Name = "order")]
    public class Order
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double TotalPrice { get; set; }

        public DateTime CreateTime { get; set; }
        public string Content { get; internal set; }
    }

    /// <summary>
    /// http://blog.csdn.net/xialei199023/article/details/48227247
    /// http://blog.csdn.net/xialei199023/article/details/48298635
    /// http://www.cnblogs.com/xing901022/p/5339419.html
    /// </summary>
    public class OrderCollection
    {
        /// <summary>
        /// 集群健康状况
        /// </summary>
        public static readonly string Health = "http://172.16.36.130:9200/_cluster/health";

        /// <summary>
        /// 创建主从分片（主分片与从分片，从分片可以用来部署集群的时候，分配给从机）
        /// </summary>
        public static readonly string Shards = "PUT /blogs{settings: {number_of_shards: 3,number_of_replicas : 1 }}";

        /// <summary>
        /// 自定义Id
        /// </summary>
        public static readonly string PutUp = "Put /megacorp/employee/1{ \"first_name\" :  \"Hi\",\"last_name\" :   \"Kitte\",\"age\" :         35, \"about\":        \"I like to build cabinets\",\"interests\":  [ \"forestry\" ]}";

        /// <summary>
        /// 自生成Id
        /// </summary>
        public static readonly string PostUp = "POST /megacorp/employee{ \"first_name\" :  \"Hi\",\"last_name\" :   \"Kitte\",\"age\" :         35, \"about\":        \"I like to build cabinets\",\"interests\":  [ \"forestry\" ]}";

        /// <summary>
        /// Delete(删除整个索引）
        /// </summary>
        public static readonly string DeleteIndex = "http://172.16.36.130:9200/blogs";
        /// <summary>
        /// Get
        /// </summary>
        public static readonly string Search = "GET /megacorp/employee/_search";

        /// <summary>
        /// basic Search
        /// </summary>
        public static readonly string SearchBasic = "Post{\"query\":{\"match\":{\"first_name\":\"John\"}}}";

        /// <summary>
        /// 组合条件用到bool Must Filter
        /// </summary>
        public static readonly string ComplexSearch = "Post {\"query\" : {\"bool\": { \"must\": {\"match\" : {\"last_name\" : \"smith\" }},\"filter\": {\"range\" : {\"age\" : { \"gt\" : 30 } } } } }";

        /// <summary>
        /// FUll Search
        /// </summary>
        public static readonly string FullSearch = "post {\"query\" : { \"match\" : { \"about\" : \"rock climbing\"}}}";

        /// <summary>
        /// Full Search Phrase Search
        /// </summary>
        public static readonly string FullSeachPhrase = "Post {\"query\" : { \"match_phrase\" : { \"about\" : \"rock climbing\"}}}";

        /// <summary>
        /// Highlight 搜索的元素以<em></em>元素包裹
        /// </summary>
        public static readonly string FullSearchRelationHigh = "{\"query\" : { \"match_phrase\" : {\"about\" : \"rock climbing\"}},\"highlight\": {\"fields\" : { \"about\" : {} }}}";

        /// <summary>
        ///Edit 之后 Version 会变化
        /// </summary>
        public static readonly string Edit = "PUT /website/blog/123{\"title\": \"My first blog entry\",\"text\":  \"I am starting to get the hang of this...\",\"date\":  \"2014/01/02\"}";

        /// <summary>
        /// 配上version=1 提示更新的数据 ，如遇更改之后会报conflict 错误哈
        /// </summary>
        public static readonly string ConflictEdit = "Put http://172.16.36.130:9200/megacorp/employee/3?version=1";

        /// <summary>
        /// 局部更新必须加上_update 必须被doc包裹
        /// </summary>
        public static readonly string UpdatePart = "POST /website/blog/1/_update {\"doc\" : {\"tags\" : [ \"testing\" ], \"views\": 0}}";

        /// <summary>
        /// http://blog.csdn.net/xialei199023/article/details/48298635
        /// metric（度量）聚合
        /// 最小化聚合（min) 最大化聚合max 求数值（sum) 求平均（avg) 统计聚合 （stats）
        /// 
        /// bucketing（桶）聚合
        /// </summary> 
        public static readonly string Aggrateions = "Post /megacorp/employee/_search {\"aggregations\":{\"min_age\":{\"min\":{\"field\":\"age\"}}}}";

        /// <summary>
        /// 根据排序聚合，取最小条数
        /// </summary>
        public static readonly string TopHitAggrataions = "Post {\"aggregations\": {\"top_age\": {\"top_hits\": {\"sort\": [{\"age\": {            \"order\": \"desc\" }}], \"_source\": { \"include\": [ \"first_name\", \"age\" ] },\"size\": 2                } } }}";

        /// <summary>
        /// FildData 错误更改属性
        /// </summary>
        public static readonly string Fildate = "Post /megacorp/_mapping/employee {\"properties\": {\"first_name\": { \"type\":     \"text\",\"fielddata\": true}}}";

        /// <summary>
        /// MergeQUery
        /// </summary>
        public static readonly string MergeQuery = "Post megacorp/employee/_mget {\"docs\" : [{ \"_index\" : \"megacorp\", \"_type\" :  \"employee\",\"_id\" :    2 },{\"_index\" : \"megacorp\",\"_type\" :  \"employee\",\"_id\" :    1 }]}";

        /// <summary>
        /// 分词分析器分析
        /// </summary>
        public static readonly string Analysi= "Post http://172.16.36.130:9200/megacorp/_analyze {\"analyzer\": \"standard\",\"text\": \"Text to analyze\"}";

        /// <summary>
        /// 相似分析器
        /// </summary>
        public static readonly string Explain = "Post http://172.16.36.130:9200/megacorp/employee/1/_explain {\"query\":{\"match\":{\"first_name\":\"John\"}}}";

        /// <summary>
        /// 查看映射的类型
        /// </summary>
        public static readonly string Mapping = "Get http://172.16.36.130:9200/megacorp/_mapping/employee";

        /// <summary>
        /// 针对新建索引新建mapping
        /// </summary>
        public static readonly string CreateMapping = "Put http://172.16.36.130:9200/gb {" +
                                        " \"mappings\": {" +
                                        " \"tweet\" : {" +
                                      "\"properties\" : {" +
                                       "\"tweet\" : {" +
                                          "\"type\" :    \"string\"," +
                                          "\"analyzer\": \"english\"+},\"date\" : { \"type\" :   \"date\"},\"name\" : { \"type\" :   \"string\"},\"user_id\" : { \"type\" :   \"long\" } }}}}";

        /// <summary>
        /// 添加已存在的索引列类型mapping，添加新的mapping
        /// 但是不允许修改已存在建立的mapping，因为里面涉及到索引的建立等问题吧
        /// </summary>
        public static readonly string UpdateMapping = "put http://172.16.36.130:9200/gb/_mapping/tweet {\"properties\" : { \"tag\" : {\"type\" :    \"string\",\"index\":    \"not_analyzed\"}}}";

        public static readonly string Object = "Elastic 不理解 object ，所以需要把对象建立为二级域名形式哈";

        /// <summary>
        /// 查询是否合法
        /// </summary>
        public static readonly string queryValidate = "Get http://172.16.36.130:9200/gb/tweet/_validate/query";

        /// <summary>
        /// 查询合法不合法原因
        /// </summary>
        public static readonly string queryValidateExplain = "  http://172.16.36.130:9200/gb/tweet/_validate/query?explain";

        /// <summary>
        /// 一个属性建立多个索引
        /// </summary>
        public static readonly string createIndexMultiAnaly = "Put http://172.16.36.130:9200/my_index {   \"mappings\": {     \"my_type\": {\"properties\": { \"title\": {  \"type\":     \"string\",\"analyzer\": \"english\", \"fields\": { \"std\":   { \"type\":     \"string\",\"analyzer\": \"standard\" }}} } } }}";

        /// <summary>
        /// 增加搜索的概率，一个字段运用多个分词规则
        /// </summary>
        public static readonly string MultiSearchByMultiAnaly = " POST http://172.16.36.130:9200/my_index {\"query\": {\"multi_match\": {\"query\":  \"jumping rabbits\",\"type\":   \"most_fields\", \"fields\": [ \"title\", \"title.std\" ]}}}";

        /// <summary>
        /// like 查询
        /// </summary>
        public static readonly string PreFixMoHUReglex = "130_Partial_Matching";

        /// <summary>
        /// 插件marven
        /// </summary>
        public static readonly string Marven = "https://www.elastic.co/guide/en/marvel/current/getting-started.html";

        /// <summary>
        /// node 统计信息
        /// </summary>
        public static readonly string HealthNode = "http://172.16.36.130:9200/_nodes/stats";

        /// <summary>
        /// 集群统计信息
        /// </summary>
        public static readonly string CLusterStatic = "http://172.16.36.130:9200/_cluster/stats";
    }
}
