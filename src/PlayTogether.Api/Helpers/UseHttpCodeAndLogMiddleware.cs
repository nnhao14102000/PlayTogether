using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using PlayTogether.Core.Exceptions;
using System;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace PlayTogether.Api.Helpers
{
    public static class HttpStatusCodeExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpCodeAndLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpCodeAndLogMiddleware>();
        }
    }

    public class HttpCodeAndLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpCodeAndLogMiddleware> _logger;

        public HttpCodeAndLogMiddleware(RequestDelegate next, ILogger<HttpCodeAndLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) {
                return;
            }

            try {
                httpContext.Request.EnableBuffering();
                await _next(httpContext);
            }
            catch (Exception exception) {
                var response = httpContext.Response;
                response.ContentType = "application/json";

                switch (exception) {
                    case ApiException e:
                        // custom application error
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await WriteAndLogResponseAsync(exception, httpContext, HttpStatusCode.BadRequest, LogLevel.Error, "BadRequest Exception!" + e.Message);
                        break;
                    case NotFoundException e:
                        // not found error
                        httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        await WriteAndLogResponseAsync(exception, httpContext, HttpStatusCode.NotFound, LogLevel.Error, "Not Found!" + e.Message);
                        break;
                    case ValidationException e:
                        // Validation error
                        httpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        await WriteAndLogResponseAsync(exception, httpContext, HttpStatusCode.UnprocessableEntity, LogLevel.Error, "Validation Exception!" + e.Message);
                        break;
                    case AuthenticationException e:
                        // Authentication error
                        httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await WriteAndLogResponseAsync(exception, httpContext, HttpStatusCode.Unauthorized, LogLevel.Error, "Authentication Exception!" + e.Message);
                        break;
                    default:

                        // Unhandled error
                        //httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await WriteAndLogResponseAsync(exception, httpContext, HttpStatusCode.UnprocessableEntity, LogLevel.Error, "Server error!");
                        break;
                }
            }
        }

        private async Task WriteAndLogResponseAsync(
            Exception exception,
            HttpContext httpContext,
            HttpStatusCode httpStatusCode,
            LogLevel logLevel,
            string alternateMessage = null)
        {
            string requestBody = string.Empty;
            if (httpContext.Request.Body.CanSeek) {
                httpContext.Request.Body.Seek(0, System.IO.SeekOrigin.Begin);
                using (var sr = new System.IO.StreamReader(httpContext.Request.Body)) {
                    requestBody = JsonConvert.SerializeObject(sr.ReadToEndAsync());
                }
            }

            StringValues authorization;
            httpContext.Request.Headers.TryGetValue("Authorization", out authorization);

            var customDetails = new StringBuilder();
            customDetails
                .AppendFormat("\n   Service URL      :").Append(httpContext.Request.Path.ToString())
                .AppendFormat("\n   Request Method   :").Append(httpContext.Request?.Method)
                .AppendFormat("\n   Request Body     :").Append(requestBody)
                .AppendFormat("\n   Authorization    :").Append(authorization)
                .AppendFormat("\n   Content-Type     :").Append(httpContext.Request.Headers["Content-Type"].ToString())
                .AppendFormat("\n   Cookie           :").Append(httpContext.Request.Headers["Cookie"].ToString())
                .AppendFormat("\n   Host             :").Append(httpContext.Request.Headers["Host"].ToString())
                .AppendFormat("\n   Referer          :").Append(httpContext.Request.Headers["Referer"].ToString())
                .AppendFormat("\n   Origin           :").Append(httpContext.Request.Headers["Origin"].ToString())
                .AppendFormat("\n   User-Agent       :").Append(httpContext.Request.Headers["User-Agent"].ToString())
                .AppendFormat("\n   Error Message    :").Append(exception.Message);

            _logger.Log(logLevel, exception, customDetails.ToString());

            if (httpContext.Response.HasStarted) {
                _logger.LogError("The response has already started, the http status code middleware will not be executed.");
                return;
            }

            string responseMessage = JsonConvert.SerializeObject(
                new {
                    Message = string.IsNullOrWhiteSpace(exception.Message) ? alternateMessage : exception.Message,
                //StackTrace = string.IsNullOrWhiteSpace(alternateMessage) ? exception.StackTrace : alternateMessage
            });

            httpContext.Response.Clear();
            httpContext.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;
            httpContext.Response.StatusCode = (int)httpStatusCode;
            await httpContext.Response.WriteAsync(responseMessage, Encoding.UTF8);
        }
    }
}