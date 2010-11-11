using System;
namespace Ongle
{
	public interface IAssignExecutor
	{
		IScope Scope
		{
			get;
			set;
		}

		void Execute ( Assign info );
	}
}
