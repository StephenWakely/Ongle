using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ongle;

namespace TestProject
{
	class StandardOutDummy : IStandardOut
	{
		public string Text
		{
			get;
			set;
		}

		public void Output ( string text )
		{
			Text += text;
		}
	}
}
