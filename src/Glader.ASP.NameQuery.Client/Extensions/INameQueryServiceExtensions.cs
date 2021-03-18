using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Glader.Essentials;
using Refit;

namespace Glader.ASP.NameQuery
{
	public static class INameQueryServiceExtensions
	{
		/// <summary>
		/// Queries for a <see cref="EntityNameQueryResponse"/> from the provided <see cref="guid"/>.
		/// </summary>
		/// <param name="service">The name query service.</param>
		/// <param name="guid">The entity guid.</param>
		/// <returns>A query response.</returns>
		public static Task<EntityNameQueryResponse> QueryEntityNameAsync<TObjectGuidType>(this INameQueryService service, TObjectGuidType guid, CancellationToken token = default)
			where TObjectGuidType : BaseGuid
		{
			if (service == null) throw new ArgumentNullException(nameof(service));

			//We just simply forward this with the raw value.
			return service.QueryEntityNameAsync(guid.RawValue, token);
		}
	}
}
