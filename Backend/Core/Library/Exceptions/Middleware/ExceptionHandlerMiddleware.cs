using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Library.Exceptions.Interfaces;
using Library.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Library.Exceptions.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICurrentUserAccessor currentUserAccessor)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                List<ApplicationErrorModel> errors = new List<ApplicationErrorModel>();


                if (error is ApplicationValidationException validationException)
                {
                    errors = validationException.Errors;
                    response.StatusCode = 400;
                }
                else if (error is I401Exception)
                {
                    errors.Add(new ApplicationErrorModel { Message = error.Message, Source = error.StackTrace });

                    response.StatusCode = 401;
                }
                else
                {
                    errors.Add(new ApplicationErrorModel { Message = error.Message, Source = error.StackTrace });

                    response.StatusCode = 500;
                }


                await response.WriteAsync(
                JsonConvert.SerializeObject(ServiceResult.Failure(errors: errors)));
            }
        }
    }




    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICurrentUserAccessor currentUserAccessor)
        {
            context.User.AddIdentity(new ClaimsIdentity(new Claim[] { new Claim("id", "1") }));
            context.User.AddIdentity(new ClaimsIdentity(new Claim[] { new Claim("roleId", "1") }));
            await _next(context);
        }
    }
}


