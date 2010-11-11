using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ongle
{
	public static class StringExtensions
	{
		public static bool IsNumber ( this string possibleNumber )
		{
			double attempt;
			return double.TryParse ( possibleNumber, out attempt );
		}
	}

}
