using System;
namespace Ongle
{
	public interface IExpressionParser
	{
		Expression ParseExpression ( IScope scope, Tokens tokens );
		Expression ParseValue ( IScope scope, Tokens tokens );
		Expression ParseArray ( IScope scope, Tokens tokens );
	}
}

