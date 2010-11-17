using System;
using Ninject;

namespace Ongle
{
	public class AssignmentParser : IStatementParser
	{
		private IExecutorFactory _executorFactory;		
		private IExpressionParser _expressionParser;

		[Inject]
		public AssignmentParser (IExecutorFactory executorFactory, IExpressionParser expressionParser)
		{
			_executorFactory = executorFactory;
			_expressionParser = expressionParser;
		}
	

		#region IStatementParser implementation
		public bool TryParse (Tokens tokens, IScope scope, out IStatement statement)
		{
			tokens.SetMark ();
			
			Expression value = _expressionParser.ParseValue ( scope, tokens );
			
			// Look ahead to see if this is an assignment
			if ( value is Variable && tokens.PullTokenIfEqual ( "=" ) )
			{
				Assign assign = new Assign ( _executorFactory.GetAssignExecutor() );
				assign.Ident = value as Variable;

				Expression expression = _expressionParser.ParseExpression ( scope, tokens );
				assign.Expr = expression;
				
				statement = assign;
				return true;
			}
			
			// Failed to parse as assignment, rollback.
			tokens.RollbackToMark ();
			statement = null;
			return false;
		}
		#endregion
}
}

