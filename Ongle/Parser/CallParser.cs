using System;
using Ninject;
namespace Ongle
{
	public class CallParser : IStatementParser
	{
		private IExecutorFactory _executorFactory;		
		private IExpressionParser _expressionParser;

		[Inject]
		public CallParser (IExecutorFactory executorFactory, IExpressionParser expressionParser)
		{
			_executorFactory = executorFactory;
			_expressionParser = expressionParser;
		}
						
		#region IStatementParser implementation
		public bool TryParse (Tokens tokens, IScope scope, out IStatement statement)
		{
			// This is a block call
			Variable variable = new Variable ( _executorFactory.GetVariableExecutor() );
			variable.Scope = scope;
			variable.Ident = tokens.PullToken ();
			
			// Check if there is an indexer into the variable
			if (tokens.PeekToken () == "[")
			{
				tokens.PullToken ();
				variable.Indexer = _expressionParser.ParseExpression ( scope, tokens );			
				tokens.RemoveNextToken ("]");
			}

			if ( tokens.NextTokenIs("(") )
			{
				// Parse the parameters.. 
				variable.Parameters = _expressionParser.ParseArray( scope, tokens ) as ArrayExpr;
			}
			
			statement = variable;
			return true;			
		}
		#endregion
	}
}

