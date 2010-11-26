
using System;
using Ninject;

namespace Ongle
{
	public class Variable : Expression, ITailCallExecution
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
			ExecuteWithTailCall();
		}
		
		public ITailCallExecution ExecuteWithTailCall ()
		{
			_executor.Scope = this.Scope;
			Dynamic parameters = null;
			
			if (Parameters != null )
			{
				parameters = Parameters.Evaluate();
			}
			
			return _executor.Execute ( this, parameters );
		}
	}
}
