using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using movie_rental_system.core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace movie_rental_system.api.Middleware
{
    public class APIExceptionFilter: ExceptionFilterAttribute
    {
        public readonly IWebHostEnvironment env;
        public APIExceptionFilter(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public override void OnException(ExceptionContext context)
        {
            //TODO: define a user APIException type that extends Exception class to throw exception that can add information
            // handle them here first.

            //unhandled exceptions - status code : 500 
            string message = "An unhandled exception occured";
            string stackTrace = null;

            if (this.env.IsDevelopment())
            {
                message = context.Exception.Message;
                stackTrace = context.Exception.StackTrace;
            }

            var apiError = new APIErrorDTO_Out()
            {
                Message = message,
                Status = 500,
                Details = stackTrace

            };

            context.Result = new JsonResult(apiError);

        }
    }
}
