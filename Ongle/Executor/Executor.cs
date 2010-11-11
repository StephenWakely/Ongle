using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ongle
{
	public class Executor
	{
		public Executor ( Block statements )
		{
			statements.Execute ();
		}
	}
}
