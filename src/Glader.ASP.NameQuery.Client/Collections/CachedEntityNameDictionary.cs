using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Glader.Essentials;

namespace Glader.ASP.NameQuery
{
	//Unsealed for type simplification
	/// <summary>
	/// Default implementation of <see cref="IEntityNameDictionary{TObjectGuidType,TEntityEnumType}"/>.
	/// </summary>
	/// <typeparam name="TObjectGuidType"></typeparam>
	/// <typeparam name="TEntityEnumType"></typeparam>
	public class CachedEntityNameDictionary<TObjectGuidType, TEntityEnumType> : IEntityNameDictionary<TObjectGuidType, TEntityEnumType>
		where TObjectGuidType : ObjectGuid<TEntityEnumType> 
		where TEntityEnumType : Enum
	{
		//TODO: Make this configurable.
		internal const string UKNOWN_NAME_VALUE = "Unknown";

		/// <summary>
		/// Internally mapping for <see cref="INameQueryService"/> to the type enum they can handle.
		/// </summary>
		private Dictionary<TEntityEnumType, INameQueryService> QueryServices { get; } = new Dictionary<TEntityEnumType, INameQueryService>();

		/// <summary>
		/// Internally manually managed cache that maps guids to name.
		/// </summary>
		private ConcurrentDictionary<TObjectGuidType, string> CachedNames { get; } = new ConcurrentDictionary<TObjectGuidType, string>(ObjectGuidEqualityComparer<TObjectGuidType>.Instance);

		/// <inheritdoc />
		public bool HasCached(TObjectGuidType guid)
		{
			if (guid == null) throw new ArgumentNullException(nameof(guid));

			return CachedNames.TryGetValue(guid, out _);
		}

		/// <inheritdoc />
		public string QueryEntityName(TObjectGuidType guid)
		{
			if (guid == null) throw new ArgumentNullException(nameof(guid));

			if (!HasCached(guid))
				throw new InvalidOperationException($"Unknown guid non-async requested name query. Guid: {guid}. This is not supported.");

			//TODO: Any reason to bother checking?
			CachedNames.TryGetValue(guid, out var value);
			return value;
		}

		/// <inheritdoc />
		public async Task<string> QueryEntityNameAsync(TObjectGuidType guid, CancellationToken token = default)
		{
			if (guid == null) throw new ArgumentNullException(nameof(guid));

			if (!QueryServices.ContainsKey(guid.ObjectType))
				return UKNOWN_NAME_VALUE;

			if(CachedNames.TryGetValue(guid, out var value))
				return value;

			//Don't lock around the query call because it will
			//cause significant delays for no reason, only lock when adding
			EntityNameQueryResponse response = await QueryServices[guid.ObjectType].QueryEntityNameAsync(guid, token);

			//We also don't cache failed results.
			if (response.isSuccessful)
			{
				//We don't have to bother checking if it exists (techncially a race condition)
				//because we should be replacing it with an identical response
				CachedNames.TryAdd(guid, response.Result);
				return response.Result;
			}
			else
				return UKNOWN_NAME_VALUE;
		}

		/// <inheritdoc />
		public void AddService(TEntityEnumType type, INameQueryService service)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));

			//TODO: If a resolved service goes offline we may want to change or update this??
			//Not threadsafe and we just replace.
			QueryServices[type] = service ?? throw new ArgumentNullException(nameof(service));
		}
	}
}
