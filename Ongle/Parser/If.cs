
using System;
using Ninject;

namespace Ongle
{
	public class If : Expression
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
			_executor.Scope = Scope;
			_executor.Execute ( this );
		}
	}
}
