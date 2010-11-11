
using System;
using System.Collections.Generic;

namespace Ongle
{
	public interface IValueParser
	{		
		Expression ParseValue ( IScope scope, Tokens tokens );
		Expression ParseArray ( IScope scope, Tokens tokens );

		IBlockParser BlockParser
		{
			set;
		}

	}
}
