using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace Glader.ASP.NameQuery
{
	/// <summary>
	/// Contract for REST service that provides
	/// group management.
	/// </summary>
	[Headers("User-Agent: Glader")]
	public interface INameQueryService
	{
		/// <summary>
		/// Queries for a <see cref="EntityNameQueryResponse"/> from the provided <see cref="id"/>.
		/// </summary>
		/// <param name="id">The entity id.</param>
		/// <returns>A query response.</returns>
		Task<EntityNameQueryResponse> QueryEntityNameAsync([AliasAs("id")] ulong id);
	}
}
