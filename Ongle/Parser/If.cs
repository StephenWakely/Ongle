
using System;
using Ninject;

namespace Ongle
{
	public class If : Expression, ITailCallExecution
	{
		public Expression Test;
		public Block Body;

		private IIfExecutor _executor;

		[Inject]
		public If ( IIfExecutor executor )
		{
			_executor = executor;
		}

		public override void Execute ()
		{
			ExecuteWithTailCall ();
		}
		
		public ITailCallExecution ExecuteWithTailCall ()
		{
			_executor.Scope = Scope;
			return _executor.Execute ( this );			
		}
	}
}
