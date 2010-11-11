using System;
namespace Ongle
{
	public interface IStatement
	{
		IScope Scope
		{
			get;
			set;
		}
		
		void Execute ( );
	}
}
