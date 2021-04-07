using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Glader.Essentials;

namespace Glader.ASP.NameQuery
{
	/// <summary>
	/// Entity name-query based implementation of <see cref="IServiceBaseUrlFactory"/>.
	/// </summary>
	/// <typeparam name="TEntityTypeEnum">The Entity enumeration type.</typeparam>
	public sealed class NameQueryServiceBaseUrlFactory<TEntityTypeEnum> : IServiceBaseUrlFactory
	{
		/// <summary>
		/// The enumeration value this will build name-query URLs for.
		/// </summary>
		public TEntityTypeEnum EntityTypeValue { get; }

		/// <summary>
		/// Default-base <see cref="IServiceBaseUrlFactory"/> to use.
		/// </summary>
		public DefaultServiceBaseUrlFactory DefaultUrlFactory { get; init; } = new DefaultServiceBaseUrlFactory();

		public NameQueryServiceBaseUrlFactory(TEntityTypeEnum entityTypeValue)
		{
			EntityTypeValue = entityTypeValue;
		}

		/// <inheritdoc />
		public string Create(Uri context)
		{
			if(context == null) throw new ArgumentNullException(nameof(context));

			//TODO: Refactor path building to NameQuery library
			return $"{DefaultUrlFactory.Create(context)}/api/{EntityTypeValue}Name";
		}
	}
}
