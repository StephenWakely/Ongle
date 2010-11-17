using System;
namespace Ongle
{
	public interface IStatementParser
	{
		bool TryParse (Tokens tokens, IScope scope, out IStatement statement);
	}
}

