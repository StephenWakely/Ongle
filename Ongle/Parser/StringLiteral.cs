
using System;

namespace Ongle
{
	public class StringLiteral : Expression
	{
		public string Value;
		public override Dynamic Evaluate ()
		{
			return new Dynamic ()
			{
				StringValue = Value,
				Scope = this.Scope
			};
		}
	}
}
