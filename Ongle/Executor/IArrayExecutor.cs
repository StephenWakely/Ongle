using System;
namespace Ongle
{
	public interface IArrayExecutor
	{
		IScope Scope
		{
			get;
			set;
		}

		void Execute ( Array info );
	}
}
