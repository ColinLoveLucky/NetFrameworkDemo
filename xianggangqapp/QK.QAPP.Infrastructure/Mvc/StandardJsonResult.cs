﻿using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using QK.QAPP.Infrastructure;

namespace QK.QAPP.Infrastructure
{
    public class StandardJsonResult : ActionResult, IStandardResult
    {
        #region Implementation of ICustomResult

        public bool Success { get; set; }

        public string Message { get; set; }

        public void Succeed()
        {
            this.Success = true;
        }

        public void Fail()
        {
            this.Success = false;
        }

        public void Succeed(string message)
        {
            this.Success = true;
            this.Message = message;
        }

        public void Fail(string message)
        {
            this.Success = false;
            this.Message = message;
        }

        public void Try(Action action)
        {
            try
            {
                action();
                this.Succeed();
            }
            catch (Exception ex)
            {
                this.Fail(ex.Message);
            }
        }

        #endregion

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            WriteToResponse(response);
        }

        protected virtual IStandardResult ToCustomResult()
        {
            var result = new StandardResult();
            result.Success = this.Success;
            result.Message = this.Message;
            return result;
        }

        public void ValidateModelState(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                var message = "<h5>Please clear the errors first:</h5><ul>";
                foreach (var error in modelState.Values.SelectMany(x => x.Errors))
                {
                    if (!string.IsNullOrEmpty(error.ErrorMessage))
                    {
                        message += "<li>" + error.ErrorMessage + "</li>";
                    }
                }
                message += "</ul>";
                this.Fail(message);
            }
            else
            {
                this.Succeed();
            }
        }

        public void WriteToResponse(HttpResponseBase response)
        {
            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;
            response.Write(Serializer.ToJson(this.ToCustomResult()));
        }
    }

    public class StandardJsonResult<T> : StandardJsonResult, IStandardResult<T>
    {
        public T Value { get; set; }

        protected override IStandardResult ToCustomResult()
        {
            var result =  new StandardResult<T>();
            result.Success = this.Success;
            result.Message = this.Message;
            result.Value = this.Value;
            return result;
        }
    }

}