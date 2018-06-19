using MVCDemo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MVCDemo.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {


            //ModelMetadata
            // ScaffoldColumnAttribute
            //HttpContextWrapper
            //UIHintAttribute
            /*Model*/
            //ModelMetadata
            //IModelBinder
            //ModelBindingContext
            //ValidationAttribute  
            //ValidationContext
            //ValidationResult
            //ValidationException
            //Validator

            //TypeDescriptor
            //MemberDescriptor
            //TypeDescriptionProvider
            //DataAnnotationsModelMetadataProvider

            //AssociatedMetadataTypeTypeDescriptionProvider
            //MetadataTypeAttribute
            //KeyAttribute
            //EditableAttribute
            //AssociationAttribute
            //CompareAttribute
            // ConcurrencyCheckAttribute
            //DisplayFormatAttribute
            //DataTypeAttribute
            //DisplayColumnAttribute
            //CreditCardAttribute
            //EmailAddressAttribute
            //EnumDataTypeAttribute
            //FileExtensionsAttribute
            //FilterUIHintAttribute
            //MaxLengthAttribute
            //MinLengthAttribute
            //PhoneAttribute
            //RangeAttribute
            // RegularExpressionAttribute
            // RequiredAttribute
            // ScaffoldColumnAttribute
            // ScaffoldTableAttribute
            // StringLengthAttribute
            // TimestampAttribute
            // UIHintAttribute
            //UrlAttribute
            //ValidationContext
            //ValidationException
            //ValidationResult
            //Validator
            //IValidatableObject
            //IValidatableObject  
            //IDataErrorInfo

            //ColumnAttribute
            //ComplexTypeAttribute
            //DatabaseGeneratedAttribute
            //ForeignKeyAttribute
            //InversePropertyAttribute
            //NotMappedAttribute
            //TableAttribute

            //ModelMetadataProviders
            // ModelMetadataProvider
            //ModelMetadata
            //EditorExtensions
            //HtmlHelper或者HtmlHelper<TModel>
            //DisplayExtensions
            //LabelExtensions 
            //MvcHtmlString 
            //ModelMetadata

            //AssociatedMetadataProvider
            //DataAnnotationsModelMetadataProvider 
            //DataAnnotationsModelMetadataProvider
            //CachedDataAnnotationsModelMetadataProvider


            /// ModelMetadata

            //AdditionalMetadataAttribute
            // IMetadataAware
            // IMetadataAware
            //Control

            //IController
            //ControllerBuilder
            //IControllerFactory
            //IControllerActivator
            //DefaultControllerFactory
            //ControllerBase
            //ControllerContext
            //IDependencyResolver 

            //Action
            //IActionInvoker 
            //ControllerActionInvoker
            //IModelBinder 
            //DefaultModelBinder
            //ActionResult
            // ActionResult


            // ControllerDescriptor 
            // ActionDescriptor
            //ReflectedControllerDescriptor 
            //ActionNameSelectorAttribute 
            //ActionNameAttribute 
            //ReflectedAsyncControllerDescriptor 
            //AsyncActionDescriptor 
            //ReflectedActionDescriptor
            //ActionMethodSelectorAttribute 
            //AcceptVerbsAttribute 
            //HttpGetAttribute
            //HttpPostAttribute
            //TaskAsyncActionDescriptor
            //IActionInvoker
            //IAsyncActionInvoker 
            //ControllerActionInvoker
            //AsyncControllerActionInvoker
            //ParameterDescriptor
            //ParameterBindingInfo
            //ReflectedParameterDescriptor 
            //BindAttribute 
            //IValueProvider
            //ValueProviderResult
            //NameValueCollectionValueProvider
            // IEnumerableValueProvider
            //IUnvalidatedValueProvider
            //FormValueProvider
            // QueryStringValueProvider
            // DictionaryValueProvider<TValue>
            //RouteDataValueProvider 
            // HttpFileCollectionValueProvider
            //HttpRequestBase
            // HttpFileCollectionBase 
            //ChildActionValueProvider
            //ChildActionExtensions
            //ValueProviderCollection
            //ValueProviderFactory
            //ChildActionValueProviderFactory
            //FormValueProviderFactory
            //JsonValueProviderFactory
            // RouteDataValueProviderFactory
            // QueryStringValueProviderFactory
            // HttpFileCollectionValueProviderFactory
            //ValueProviderFactories
            //ValueProviderFactoryCollection
            // DefaultModelBinder
            //IModelBinder
            //CustomModelBinderAttribute
            // ModelBinderAttribute 
            //ModelBinders
            //IModelBinderProvider
            //ModelBinderProviderCollection 


            //ModelValidator
            //ModelValidator
            //ModelClientValidationRule
            //ModelValidationResult
            // ModelClientValidationRule
            //IDataErrorInfo
            //IValidatableObject
            //ModelValidatorProvider
            //AssociatedValidatorProvider
            //DataAnnotationsModelValidatorProvider 
            //ClientDataTypeModelValidatorProvider 
            //DataErrorInfoModelValidatorProvider 
            //ModelMetadataProviders
            //ModelValidatorProviders
            // ValidationAttribute
            //DataAnnotationsModelValidator 
            // DataAnnotationsModelValidator<TAttribute>
            // RequiredAttributeAdapter
            //RangeAttributeAdapter 
            //RegularExpressionAttributeAdapter
            //StringLengthAttributeAdapter 
            //DataAnnotationsModelValidationFactory
            //DataAnnotationsModelValidatorProvider 
            //ControllerActionInvoker
            // IClientValidatable

            //Route
            //Route
            //IRouteConstraint
            //RouteDirection 
            //HttpMethodConstraint

            //HandleErrorInfo

            // UrlHelper
            //HtmlHelper
            //LinkExtensions

            //View

            //ViewResult
            // EmptyResult 
            //ContentResult
            //ControllerActionInvoker
            //FileResult
            // FileContentResult 
            //FilePathResult
            //FilePathResult
            //FileStreamResult
            //JavaScriptResult
            //JsonResult
            // HttpStatusCodeResult
            //RedirectResult
            //RedirectToRouteResult
            //IView 
            //ViewContext

            // ViewContext 
            //IViewEngine
            // ViewEngineResult
            //ViewEngines
            // ViewEngineCollection
            //WebFormViewEngine
            //RazorViewEngine
            //ViewResult 
            //WebViewPage<object>
            //RazorView
            //WebFormView
            //BuildManagerCompiledView
            //IViewPageActivator
            //IDependencyResolver 
            //DependencyResolver

            //RouteValueDictionary 
            //var b = HttpContext.Application;

            //VirtualPathProviderViewEngine

            return View();
        }
        //public string Welcome(string name)
        //{
        //    return "this is my welcome action";
        //}
        //public string Welcome(string name, int numTimes = 1)
        //{
        //    return HttpUtility.HtmlEncode(string.Format("Hello {0}, NumTimes is {1}", name, numTimes));
        //}
        public ViewResult Welcome(string name, int id = 1)
        {
            ViewBag.Name = name;
            ViewBag.Id = id;
            return View();
            // return HttpUtility.HtmlEncode("Hello " + name + ", ID: " + id);
        }
        public ViewResult First()
        {
           // Trace.Listeners.Add(new WebPageTraceListener());
            Trace.TraceError("Hello");
            Trace.Flush();
            return View();
        }
    }
}