
using System;

namespace Ongle
{
	// <int> := <digit>+
	public class NumberLiteral : Expression
	{
		public double Value
		{
			get;
			set;
		}
		
		public override Dynamic Evaluate ()
		{
			return new Dynamic ()
			{
				NumberValue = Value,
				Scope = this.Scope
			};
		}
	}
}
