using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using Newtonsoft.Json;

namespace Glader.ASP.NameQuery
{
	/// <summary>
	/// Simplified type name for <see cref="ResponseModel{TModelType,TResponseCodeType}"/> of <see cref="string"/> and
	/// <see cref="NameQueryResponseCode"/>.
	/// </summary>
	public sealed class EntityNameQueryResponse : ResponseModel<string, NameQueryResponseCode>
	{
		/// <inheritdoc />
		public EntityNameQueryResponse(NameQueryResponseCode resultCode) 
			: base(resultCode)
		{

		}

		/// <inheritdoc />
		public EntityNameQueryResponse(string result) 
			: base(result)
		{

		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		[JsonConstructor]
		public EntityNameQueryResponse()
			: base(NameQueryResponseCode.GeneralServerError)
		{
			
		}
	}
}
