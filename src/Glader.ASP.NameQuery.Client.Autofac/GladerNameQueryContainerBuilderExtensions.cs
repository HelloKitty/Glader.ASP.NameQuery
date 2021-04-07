using System;
using System.Linq;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Indexed;
using Glader.ASP.ServiceDiscovery;
using Glader.Essentials;

namespace Glader.ASP.NameQuery
{
	public static class GladerNameQueryContainerBuilderExtensions
	{
		/// <summary>
		/// Registers the specified Entity Name Dictionary type with the container.
		/// Entity Name Dictionary type must derive from <see cref="CachedEntityNameDictionary{TObjectGuidType,TEntityEnumType}"/>.
		/// </summary>
		/// <typeparam name="TDictionaryType">The dictionary type.</typeparam>
		/// <typeparam name="TGuidType">The guid type.</typeparam>
		/// <typeparam name="TEntityType">The Entity Type enumeration type.</typeparam>
		/// <param name="builder">The container builder.</param>
		/// <returns>Builder</returns>
		public static IRegistrationBuilder<TDictionaryType, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterNameDictionaryType<TDictionaryType, TGuidType, TEntityType>(this ContainerBuilder builder)
			where TDictionaryType : CachedEntityNameDictionary<TGuidType, TEntityType> 
			where TGuidType : ObjectGuid<TEntityType> 
			where TEntityType : Enum
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));

			return builder.RegisterType<TDictionaryType>()
				.As<IEntityNameDictionary<TGuidType, TEntityType>>()
				.OnActivated(args =>
				{
					IIndex<TEntityType, IServiceResolver<INameQueryService>> nameQueryServiceIndex = args
						.Context.Resolve<IIndex<TEntityType, IServiceResolver<INameQueryService>>>();

					//This registers all possibly entity types to the queryable entity dictionary
					Enum.GetValues(typeof(TEntityType))
						.Cast<TEntityType>()
						.Select(type => new { type, resolver = GetQueryServiceOrDefault(nameQueryServiceIndex, type) })
						.Where(r => r.resolver != null)
						.ToList()
						.ForEach(resolverPair =>
						{
							args.Instance.AddService(resolverPair.type, new NameQueryServiceResolverAdapter(resolverPair.resolver));
						});
				});
		}

		private static IServiceResolver<INameQueryService> GetQueryServiceOrDefault<TEntityType>(IIndex<TEntityType, IServiceResolver<INameQueryService>> nameQueryServiceIndex, TEntityType type)
		{
			if (nameQueryServiceIndex == null) throw new ArgumentNullException(nameof(nameQueryServiceIndex));
			if (type == null) throw new ArgumentNullException(nameof(type));

			nameQueryServiceIndex.TryGetValue(type, out var value);
			return value;
		}
	}
}
