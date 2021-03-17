using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;

namespace Glader.ASP.NameQuery
{
	public enum NameQueryResponseCode
	{
		Success = GladerEssentialsModelConstants.RESPONSE_CODE_SUCCESS_VALUE,

		GeneralServerError = 2,

		Invalid = 3,

		UnknownEntity = 4,

		Unsupported = 5
	}
}
