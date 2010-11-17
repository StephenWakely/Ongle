using System;
using Ninject;
namespace Ongle
{
	public class PrintParser : IStatementParser
	{
		const string PrintTag = "print";

		private IExecutorFactory _executorFactory;		
		private IExpressionParser _expressionParser;
		
		[Inject]
		public PrintParser (IExecutorFactory executorFactory, IExpressionParser expressionParser)
		{
			_executorFactory = executorFactory;
			_expressionParser = expressionParser;
		}
		
		#region IStatementParser implementation
		
		public bool TryParse (Tokens tokens, IScope scope, out IStatement statement)
		{
			if ( tokens.PeekToken () == PrintTag )
			{
				// Print statement
				tokens.RemoveNextToken ( PrintTag );
				Print print = new Print( _executorFactory.GetPrintExecutor ());
				print.Expr = _expressionParser.ParseExpression ( scope, tokens );
				
				statement = print;
				return true;
			}
			
			statement = null;
			return false;
		}
		
		#endregion		
	}
}

