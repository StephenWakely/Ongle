using System;
using Ninject;
namespace Ongle
{
	public class IfParser : IStatementParser
	{
		const string IfTag = "if";

		private IExecutorFactory _executorFactory;		
		private IExpressionParser _expressionParser;
		private IBlockParser _blockParser;

		[Inject]
		public IfParser (IExecutorFactory executorFactory, IExpressionParser expressionParser, IBlockParser blockParser)
		{
			_executorFactory = executorFactory;
			_expressionParser = expressionParser;
			_blockParser = blockParser;
		}
	

		#region IStatementParser implementation
		public bool TryParse (Tokens tokens, IScope scope, out IStatement statement)
		{
			if ( tokens.PeekToken () == IfTag ) 
			{
				tokens.RemoveNextToken ( IfTag );
				If iff = new If ( _executorFactory.GetIfExecutor ());
				iff.Test = _expressionParser.ParseExpression ( scope, tokens );
				iff.Body = _blockParser.GetBlock ( scope, tokens );
				
				statement = iff;
				return true;
			}
			
			statement = null;
			return false;
		}
		#endregion
	}
}

