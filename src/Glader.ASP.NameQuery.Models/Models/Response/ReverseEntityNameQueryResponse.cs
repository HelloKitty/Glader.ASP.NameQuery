using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;

namespace Glader.ASP.NameQuery
{
	/// <summary>
	/// Simplified type name for <see cref="ResponseModel{TModelType,TResponseCodeType}"/> of <typeparamref name="TObjectGuidType"/> and
	/// <see cref="NameQueryResponseCode"/>.
	/// </summary>
	public sealed class ReverseEntityNameQueryResponse<TObjectGuidType> : ResponseModel<TObjectGuidType, NameQueryResponseCode> 
		where TObjectGuidType : BaseGuid
	{
		public ReverseEntityNameQueryResponse(NameQueryResponseCode resultCode) 
			: base(resultCode)
		{
		}

		public ReverseEntityNameQueryResponse(TObjectGuidType result) 
			: base(result)
		{
		}
	}
}
