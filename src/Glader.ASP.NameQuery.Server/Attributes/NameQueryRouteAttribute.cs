using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Glader.ASP.NameQuery
{
	public class NameQueryRouteAttribute : RouteAttribute
	{
		public NameQueryRouteAttribute(string entityTypeName)
			: base($"api/{entityTypeName}Name")
		{
			if (string.IsNullOrWhiteSpace(entityTypeName)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(entityTypeName));
		}
	}
}
