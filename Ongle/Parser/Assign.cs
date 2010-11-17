
using System;
using Ninject;

namespace Ongle
{
	// <ident> = <expr>
	public class Assign : Expression
	{
		public Variable Ident
		{
			get;
			set;
		}
		
		public Expression Expr
		{
			get;
			set;
		}

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
