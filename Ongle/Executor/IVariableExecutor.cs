using System;
namespace Ongle
{
	public interface IVariableExecutor
	{
		IScope Scope
		{
			get;
			set;
		}

		ITailCallExecution Execute ( Variable variable, Dynamic parameters );
	}
}
