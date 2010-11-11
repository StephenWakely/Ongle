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

		void Execute ( If info );
	}
}
