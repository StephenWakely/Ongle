
using System;
using Ninject;

namespace Ongle
{
	// print <expr>
	public class Print : Expression
	{
		public Expression Expr;

		private IPrintExecutor _executor;

		[Inject]
		public Print ( IPrintExecutor executor )
		{
			_executor = executor;
		}

		public override void Execute ( )
		{
			_executor.Scope = Scope;
			_executor.Execute ( this );
		}
	}
}
