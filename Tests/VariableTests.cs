
using System;
using NUnit.Framework;
using Ongle;

namespace Tests
{


	[TestFixture()]
	public class VariableTests
	{

		[Test()]
		public void TestParseVariableWithArrayIndex ()
		{
			Tokens tokens = new Tokens();
			tokens.Add('x');
			tokens.Add('[');
			tokens.Add('0');
			tokens.Add(']');
			
			
		}
	}
}
