using System;
namespace Ongle
{
	public interface IDeclareVarExecutor
	{
		IScope Scope
		{
			get;
			set;
		}

		void Execute ( DeclareVar info );
	}
}
