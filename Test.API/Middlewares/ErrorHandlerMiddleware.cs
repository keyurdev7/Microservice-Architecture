#region namespace
using Test.Application.Exceptions;
using Test.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ValidationException = Test.Application.Exceptions.ValidationException;
#endregion

namespace Test.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                //Log Error
                var request = await FormatRequest(context.Request);
                var requestName = typeof(IRequest).Name;
                _logger.LogError(error, "Application Request: Exception for Request {Name} {@Request}", requestName, request);

                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { succeeded = false, message = "" };
                responseModel.errors = new List<ErrorModel>();

                switch (error)
                {
                    case FluentValidation.ValidationException e:
                        var errordata = ((FluentValidation.ValidationException)error).Errors;
                        if (errordata != null)
                        {
                            foreach (var item in errordata)
                            {
                                responseModel.errors.Add(new ErrorModel() { errorMessage = item.ErrorMessage, propertyName = item.PropertyName });
                            }
                        }
                        break;
                    case ApiException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.errors.Add(new ErrorModel() { errorMessage = error.Message, propertyName = "" });
                        break;

                    case ValidationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        responseModel.errors = e.Errors;
                        break;

                    case NotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.errors.Add(new ErrorModel() { errorMessage = error.Message, propertyName = "" });
                        break;

                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.errors.Add(new ErrorModel() { errorMessage = error.Message, propertyName = "" });
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.errors.Add(new ErrorModel() { errorMessage = error.Message, propertyName = "" });
                        break;
                }

                responseModel.transactionDateTime = DateTime.UtcNow;
                var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                await response.WriteAsync(result);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            // Leave the body open so the next middleware can read it.
            using var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            // Do some processing with body…

            var formattedRequest = $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {body}";

            // Reset the request body stream position so the next middleware can read it
            //request.Body.Position = 0;

            return formattedRequest;
        }
    }
}
