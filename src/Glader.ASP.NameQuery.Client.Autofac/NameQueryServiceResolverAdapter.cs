using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Glader.ASP.ServiceDiscovery;

namespace Glader.ASP.NameQuery
{
	/// <summary>
	/// Adapter for <see cref="IServiceResolver{TServiceInterfaceType}"/> for <see cref="INameQueryService"/>
	/// </summary>
	public sealed class NameQueryServiceResolverAdapter : INameQueryService
	{
		/// <summary>
		/// Internal resolvable instance for the <see cref="INameQueryService"/>.
		/// </summary>
		private IServiceResolver<INameQueryService> ServiceResolver { get; }

		public NameQueryServiceResolverAdapter(IServiceResolver<INameQueryService> serviceResolver)
		{
			ServiceResolver = serviceResolver ?? throw new ArgumentNullException(nameof(serviceResolver));
		}

		/// <inheritdoc />
		public async Task<EntityNameQueryResponse> QueryEntityNameAsync(ulong id, CancellationToken token = default)
		{
			ServiceResolveResult<INameQueryService> result = await ServiceResolver.Create(token);

			if(result.isAvailable)
				return await result.Instance.QueryEntityNameAsync(id, token);

			return new EntityNameQueryResponse(NameQueryResponseCode.GeneralServerError);
		}
	}
}
