
using System;
using Ninject;

namespace Ongle
{
	// <ident> = <expr>
	public class Assign : Expression
	{
		public string Ident;
		public Expression Expr;

		private IAssignExecutor _executor;

		public Assign ( IAssignExecutor executor )
		{
			_executor = executor;
		}

		public override void Execute ()
		{
			_executor.Scope = Scope;
			_executor.Execute ( this );
		}
	}
}
