using System;
namespace Ongle
{
	public interface IPrintExecutor
	{
		IScope Scope
		{
			get;
			set;
		}

		void Execute ( Print info );
	}
}
