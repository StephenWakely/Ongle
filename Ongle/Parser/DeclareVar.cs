
using System;
using Ninject;

namespace Ongle
{

	// var <ident> = <expr>
	public class DeclareVar : Expression
	{
		public string Ident;
		public Expression Expr;
		private IDeclareVarExecutor _executor;

		[Inject]
		public DeclareVar ( IDeclareVarExecutor executor )
		{
			_executor = executor;
		}

		public override void Execute ( )
		{
			_executor.Execute ( this );
		}
	}
}
