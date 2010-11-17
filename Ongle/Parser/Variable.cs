
using System;
using Ninject;

namespace Ongle
{
	// <ident> := <char> <ident_rest>*
	// <ident_rest> := <char> | <digit>
	public class Variable : Expression
	{
		public string Ident { get; set; }		
		public Expression Indexer { get; set; }
		public ArrayExpr Parameters { get; set; }
		
		private IVariableExecutor _executor;

		public Variable ( IVariableExecutor executor )
		{
			_executor = executor;
		}

		public override Dynamic Evaluate ()
		{
			Dynamic dynamic = this.Scope.GetDynamic ( Ident );
			
			if (dynamic.Type == DynamicType.arrayType)
			{				
				Dynamic index = Indexer.Evaluate();
				return dynamic.ArrayValue[(Int32)Math.Truncate (index.NumberValue)];
			}
			
			return dynamic;
		}

		public override void Execute ()
		{
			_executor.Scope = this.Scope;
			Dynamic parameters = null;
			
			if (Parameters != null )
			{
				parameters = Parameters.Evaluate();
			}
			
			_executor.Execute ( this, parameters );
		}
	}
}
