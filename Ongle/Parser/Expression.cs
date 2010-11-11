
using System;

namespace Ongle
{
	public abstract class Expression : IStatement
	{
//		protected IScope _heap;
		
		public virtual IScope Scope
		{
			get;
			set;
		}
		
		public virtual Dynamic Evaluate ()
		{
			return null;
		}

		public virtual void Execute ()
		{
			Evaluate ();
		}
	}
}
