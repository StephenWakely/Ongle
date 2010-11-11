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

		void Execute ( Variable variable );
	}
}
