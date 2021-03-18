using System;
using System.Threading;
using System.Threading.Tasks;
using Glader.Essentials;

namespace Glader.ASP.NameQuery
{
	/// <summary>
	/// Contract for types that can provide mapping from <typeparamref name="TObjectGuidType"/> to string name.
	/// </summary>
	/// <typeparam name="TObjectGuidType"></typeparam>
	/// <typeparam name="TEntityEnumType"></typeparam>
	public interface IEntityNameDictionary<in TObjectGuidType, in TEntityEnumType>
		where TObjectGuidType : BaseGuid
	{
		/// <summary>
		/// Indicates if the dictionary has the provided guid cached.
		/// </summary>
		/// <param name="guid">The guid to check the cache for.</param>
		/// <returns>True if the dictionary has the guid name cached.</returns>
		bool HasCached(TObjectGuidType guid);

		/// <summary>
		/// Non-async name query that should only be called after checking <see cref="HasCached"/>.
		/// Will throw if <see cref="HasCached"/> was false.
		/// </summary>
		/// <param name="guid">The entity guid.</param>
		/// <exception cref="InvalidOperationException">Throws if <see cref="HasCached"/> is false for this guid.</exception>
		/// <returns>The query response.</returns>
		string QueryEntityName(TObjectGuidType guid);

		/// <summary>
		/// Queries for entity's name from the dictionary async.
		/// </summary>
		/// <param name="guid">The entity guid.</param>
		/// <param name="token">Cancel token.</param>
		/// <returns>The query response.</returns>
		Task<string> QueryEntityNameAsync(TObjectGuidType guid, CancellationToken token = default);

		/// <summary>
		/// Registers a name <see cref="INameQueryService"/> to handle entities of type <see cref="type"/>.
		/// (Not promised to be thread-safe)
		/// </summary>
		/// <param name="type">The entity type.</param>
		/// <param name="service">The query service.</param>
		void AddService(TEntityEnumType type, INameQueryService service);
	}
}