using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Photograph.WebApi.Exceptions;

namespace Photograph.WebApi.Interceptors
{
	public class ExceptionFilter : ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			var exception = actionExecutedContext.Exception;

			if (exception is SuspendedUserException)
			{
				actionExecutedContext.Response = GetResponse(exception.Message, HttpStatusCode.Forbidden);
			}
			else
			{
				actionExecutedContext.Response = GetResponse("500 Internal Server Error", HttpStatusCode.InternalServerError);
			}

			//throw new HttpResponseException(actionExecutedContext.Response);
		}

		private static HttpResponseMessage GetResponse(string message, HttpStatusCode statusCode)
		{
			return new HttpResponseMessage()
			{
				Content = new StringContent(message, System.Text.Encoding.UTF8, "text/plain"),
				StatusCode = statusCode
			};
		}
	}
}