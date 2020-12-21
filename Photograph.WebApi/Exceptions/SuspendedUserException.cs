using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photograph.WebApi.Exceptions
{
	public class SuspendedUserException : Exception
	{
		public SuspendedUserException() : base("101")
		{
		}

		public SuspendedUserException(string message) : base(message)
		{
		}
	}
}