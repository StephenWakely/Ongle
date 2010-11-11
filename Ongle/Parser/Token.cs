using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ongle
{
	public class Token
	{
		public string Value
		{
			get;
			set;
		}

		public Token ( string value )
		{
			this.Value = value;
		}
	}
}
