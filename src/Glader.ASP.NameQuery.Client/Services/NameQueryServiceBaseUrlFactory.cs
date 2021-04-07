using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Glader.Essentials;

namespace Glader.ASP.NameQuery
{
	public sealed class NameQueryServiceBaseUrlFactory<TEntityTypeEnum> : IServiceBaseUrlFactory
	{
		public TEntityTypeEnum EntityTypeValue { get; }

		public DefaultServiceBaseUrlFactory DefaultUrlFactory { get; init; } = new DefaultServiceBaseUrlFactory();

		public NameQueryServiceBaseUrlFactory(TEntityTypeEnum entityTypeValue)
		{
			EntityTypeValue = entityTypeValue;
		}

		public string Create(Uri context)
		{
			if(context == null) throw new ArgumentNullException(nameof(context));

			//TODO: Refactor path building to NameQuery library
			return $"{DefaultUrlFactory.Create(context)}/api/{EntityTypeValue}Name";
		}
	}
}
