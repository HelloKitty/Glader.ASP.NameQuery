using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glader.Essentials;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Glader.ASP.NameQuery
{
	public abstract class BaseNameQueryController<TObjectGuidType> : AuthorizationReadyController, INameQueryService
		where TObjectGuidType : BaseGuid
	{
		/// <inheritdoc />
		protected BaseNameQueryController(IClaimsPrincipalReader claimsReader, ILogger<AuthorizationReadyController> logger)
			: base(claimsReader, logger)
		{

		}

		//TODO: If result is a failure we must not set cache
		[AllowAnonymous]
		[ProducesJson]
		[ResponseCache(Duration = 360)] //We want to cache this for a long time. But it's possible with name changes that we want to not cache forever
		[HttpGet("{id}/name")]
		public async Task<EntityNameQueryResponse> QueryEntityNameAsync([FromRoute(Name = "id")] ulong id)
		{
			//TODO: This is a low approach
			//See benchmarks: https://github.com/KSemenenko/CreateInstance
			TObjectGuidType guid = (TObjectGuidType)Activator.CreateInstance(typeof(TObjectGuidType), new object[] {id});

			//Since this is a GET we can't send a JSON model. We have to use this process instead, sending the raw guid value.
			ResponseModel<string, NameQueryResponseCode> result = await QueryEntityNameAsync(guid);

			if (result is EntityNameQueryResponse castedResult)
				return castedResult;

			return result.isSuccessful ? new EntityNameQueryResponse(result.Result) : new EntityNameQueryResponse(result.ResultCode);
		}

		//TODO: If result is a failure we must not set cache
		[AllowAnonymous]
		[ProducesJson]
		[ResponseCache(Duration = 360)] //We want to cache this for a long time. But it's possible with name changes that we want to not cache forever
		[HttpGet("{name}/reverse")]
		public async Task<ReverseEntityNameQueryResponse<TObjectGuidType>> ReverseQueryEntityNameAsync([FromRoute(Name = "name")] string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return new ReverseEntityNameQueryResponse<TObjectGuidType>(NameQueryResponseCode.Invalid);

			var result = await QueryEntityGuidAsync(name);

			if(result is ReverseEntityNameQueryResponse<TObjectGuidType> castedResult)
				return castedResult;

			return result.isSuccessful ? new ReverseEntityNameQueryResponse<TObjectGuidType>(result.Result) : new ReverseEntityNameQueryResponse<TObjectGuidType>(result.ResultCode);
		}

		/// <summary>
		/// Implementer should implement name query logic and produce a result for the query
		/// based on the provided <see cref="ObjectGuid{TEntityType}"/>.
		/// </summary>
		/// <param name="guid">The guid to query a name for.</param>
		/// <returns>A name query response model.</returns>
		protected abstract Task<ResponseModel<string, NameQueryResponseCode>> QueryEntityNameAsync(TObjectGuidType guid);

		/// <summary>
		/// Implementer should implement reverse name query logic and produce a result for the query
		/// based on the provided <see cref="name"/>. Not all entity types may map 1:1 from name to guid so
		/// it's acceptable for the implementer to return a result indicating this.
		/// </summary>
		/// <param name="name">The entity name.</param>
		/// <returns>A reverse name query response model.</returns>
		protected virtual Task<ResponseModel<TObjectGuidType, NameQueryResponseCode>> QueryEntityGuidAsync(string name)
		{
			return Task.FromResult(new ResponseModel<TObjectGuidType, NameQueryResponseCode>(NameQueryResponseCode.Unsupported));
		}
	}
}
