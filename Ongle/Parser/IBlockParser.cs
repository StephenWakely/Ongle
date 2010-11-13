
using System;
using System.Collections.Generic;

namespace Ongle
{

	public interface IBlockParser
	{
	 	Block GetBlock ( IScope scope, Tokens tokens );
		void ParseBlock ( Block block, Tokens tokens );
		
		List<IStatementParser> Parsers
		{
			get;
		}
	}
}
