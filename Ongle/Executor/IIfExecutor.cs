using System;
namespace Ongle
{
	public interface IIfExecutor
	{
		IScope Scope
		{
			get;
			set;
		}

		ITailCallExecution Execute ( If info );
	}
}
