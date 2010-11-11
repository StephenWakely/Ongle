using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ongle
{
	public class StandardOut : Ongle.IStandardOut
	{
		public void Output ( string text )
		{
			Console.Write ( text );
		}
	}
}
