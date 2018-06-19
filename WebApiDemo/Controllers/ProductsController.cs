using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Dispatcher;
using System.Web.Http.Metadata;
using System.Web.Http.ModelBinding;
using System.Web.Http.Routing;
using System.Web.Http.ValueProviders;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    public class ProductsController : ApiController
    {
        //HttpMessageHandler
        //HttpControllerDispatcher
        //IHttpControllerSelector
        //IHttpControllerTypeResolver
        //IAssembliesResolver
        // IHttpControllerActivator
        //IHttpActionSelector
        //HttpActionDescriptor
        // HttpActionBinding
        //HttpParameterBinding
        //HttpParameterDescriptor
        // ModelMetadataProvider
        // ModelMetadata
        //ModelValidator
        //ModelValidatorProvider

        //IFilter
        //HttpActionContext
        //IAuthorizationFilter
        //AuthorizationFilterAttribute
        //Authorize

        // ModelBinderParameterBinding
        // IModelBinder
        //ModelBindingContext
        //FormatterParameterBinding
        //HttpParameterDescriptor
        //MediaTypeFormatter
        //MediaTypeMapping
        //IValueProvider
        //ValueProviderResult
        //IActionHttpMethodProvider

        //DefaultHttpControllerSelector 
        //Route
        //HttpPost
        //HttpGet
        //RoutePrefix
        //IHttpRouteConstraint
        //ResponseType

        Product[] products = new Product[] 
        { 
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 }, 
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M }, 
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M } 
        };

        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }
        public IHttpActionResult GetProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (products == null)
                return NotFound();
            return Ok(product);
        }
        public void GetVoid()
        {

        }
        public HttpResponseMessage GetResponseData()
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");

            response.Content = new StringContent("hello", Encoding.Unicode);

            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromMinutes(20)
            };

            return response;
        }
        public IHttpActionResult GetTextResult(string value)
        {
            return new TextResult(value, Request);
        }

        [HttpGet]
        [ActionName("GetAllProductDetails")]
        [NonAction]
        public HttpResponseMessage Details()
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, products);

            return response;
        }

        [Route("All/{productId}/details")]
        public IEnumerable<Product> GetProductById(int productId)
        {
            return products.Where(x => x.Id == productId).ToList();
        }

        [Route("All/{id:int:max(3)}")]
        public IEnumerable<Product> GetProductByIntId(int id)
        {
            return products.Where(x => x.Id == id).ToList();
        }

        [Route("All/Details/{id:nonzero}")]
        public HttpResponseMessage GetNoneZero(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, id);
        }

        [Route("api/p",Name="RoutName")]
        public HttpResponseMessage GetProductRoutName()
        {
            HttpResponseMessage response= Request.CreateResponse(HttpStatusCode.OK, products);

            string uri = Url.Link("RoutName", new { id = 1 });
            response.Headers.Location = new Uri(uri);

            return response;
        }

        [Route("pending", Order = 1)]
        public HttpResponseMessage GetPending()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "1");
        }

    }

    public class TextResult : IHttpActionResult
    {
        string _value;
        HttpRequestMessage _request;
        public TextResult(string value, HttpRequestMessage request)
        {
            _value = value;
            _request = request;
        }
        public Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage
            {
                Content = new StringContent(_value),
                RequestMessage = _request
            };

            return Task.FromResult(response);
        }
    }

    public class NonZeroConstraint : IHttpRouteConstraint
    {
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            object value;
            if (values.TryGetValue(parameterName, out value) && value != null)
            {
                long longValue;
                if (value is long)
                {
                    longValue = (long)value;
                    return longValue != 0;
                }

                string valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
                if (Int64.TryParse(valueString, NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out longValue))
                {
                    return longValue != 0;
                }
            }
            return false;
        }
    }


}
